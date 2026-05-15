using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.RequestModel;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Infrastructure.BaleBot.Models;
using Coreapi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Coreapi.Infrastructure.BaleBot;

public class BaleService(
    UserManager<ApplicationUser> userManager,
    IElectProjectRepository electProjectRepository,
    IElectProjectFileRepository electProjectFileRepository,
    IS3Service s3Service,
    IHttpClientFactory httpClientFactory,
    BaleConversationStateManager stateManager,
    IEngineerRepository engineerRepository,
    IElectProjectProcessRepository electProjectProcessRepository,
    ILogger<BaleService> logger) : IBaleService
{
    // S3 folder prefix used by ElectProject file uploads
    private const string ElectProjectS3Prefix = "Upload/ElectProjects";

    // Callback data prefixes / constants
    private const string FileCallbackPrefix    = "file:";
    private const string ProjectCallbackPrefix = "project:";
    private const string LoginUserCallback     = "login:user";
    private const string LoginLandlordCallback = "login:landlord";
    private const string MenuFileNumberCallback   = "menu:file_number";
    private const string MenuElectRequestCallback = "menu:elect_request";
    private const string MenuEngProjectsCallback  = "menu:eng_projects";
    private const string MenuBackCallback         = "menu:back";

    private static readonly string[] AdminRoles =
        ["Administrator", "ElectAdmin", "Elect", "Section"];

    // ── Public entry points ───────────────────────────────────────────────────

    public async Task ProcessUpdateAsync(long chatId, string text, CancellationToken cancellationToken = default)
    {
        text = text?.Trim() ?? string.Empty;

        // /start always resets and shows the initial menu
        if (text == "/start")
        {
            stateManager.Reset(chatId);
            await ShowInitialMenuAsync(chatId, cancellationToken);
            return;
        }

        var state = stateManager.GetOrCreate(chatId);

        // Check if user is already authenticated
        var existingUser = await userManager.Users
            .FirstOrDefaultAsync(u => u.BaleId == chatId.ToString(), cancellationToken);

        if (existingUser != null)
        {
            // Recover from stale pre-auth stages (e.g. bot restarted mid-login)
            if (state.Stage is BaleConversationStage.WaitingForUsername
                             or BaleConversationStage.WaitingForPassword
                             or BaleConversationStage.SelectingLoginType)
            {
                state.Stage = BaleConversationStage.Authenticated;
                state.PendingUsername = null;
            }

            switch (state.Stage)
            {
                case BaleConversationStage.WaitingForFileNumber:
                    await HandleFileNumberInputAsync(chatId, state, text, existingUser, cancellationToken);
                    break;
                case BaleConversationStage.WaitingForElectRequestNum:
                    await HandleElectRequestNumInputAsync(chatId, state, text, cancellationToken);
                    break;
                default:
                    // Any other text from an authenticated user → re-show their menu
                    await ShowRoleMenuAsync(chatId, existingUser, cancellationToken);
                    break;
            }
            return;
        }

        // Unauthenticated – route by conversation stage
        switch (state.Stage)
        {
            case BaleConversationStage.SelectingLoginType:
                await ShowInitialMenuAsync(chatId, cancellationToken);
                break;

            case BaleConversationStage.WaitingForUsername:
                if (string.IsNullOrEmpty(text))
                    await SendMessageAsync(chatId, "لطفاً نام کاربری خود را وارد کنید:", cancellationToken);
                else
                {
                    state.PendingUsername = text;
                    state.Stage = BaleConversationStage.WaitingForPassword;
                    await SendMessageAsync(chatId, "لطفاً رمز عبور خود را وارد کنید:", cancellationToken);
                }
                break;

            case BaleConversationStage.WaitingForPassword:
                await HandleLoginAsync(chatId, state, text, cancellationToken);
                break;

            case BaleConversationStage.WaitingForLandlordNaCode:
                await HandleLandlordNaCodeInputAsync(chatId, text, cancellationToken);
                break;

            default:
                await ShowInitialMenuAsync(chatId, cancellationToken);
                break;
        }
    }

    public async Task HandleCallbackQueryAsync(long chatId, string callbackQueryId, string data,
        CancellationToken cancellationToken = default)
    {
        // Always dismiss the loading indicator on the button first
        await AnswerCallbackQueryAsync(callbackQueryId, cancellationToken);

        var state = stateManager.GetOrCreate(chatId);

        // ── Login-type selection (works for unauthenticated users) ────────────
        if (data == LoginUserCallback)
        {
            state.Stage = BaleConversationStage.WaitingForUsername;
            state.PendingUsername = null;
            await SendMessageAsync(chatId, "لطفاً نام کاربری خود را وارد کنید:", cancellationToken);
            return;
        }

        if (data == LoginLandlordCallback)
        {
            state.Stage = BaleConversationStage.WaitingForLandlordNaCode;
            await SendMessageAsync(chatId, "لطفاً کد ملی مالک را وارد کنید:", cancellationToken);
            return;
        }

        // ── Project / file callbacks — open to everyone (including landlords) ──
        if (data.StartsWith(ProjectCallbackPrefix) &&
            long.TryParse(data[ProjectCallbackPrefix.Length..], out var fileNumber))
        {
            await ShowProjectWithFilesAsync(chatId, fileNumber, cancellationToken);
            await SendBackButtonAsync(chatId, cancellationToken);
            return;
        }

        if (data.StartsWith(FileCallbackPrefix) &&
            Guid.TryParse(data[FileCallbackPrefix.Length..], out var fileId))
        {
            await DownloadFileAsync(chatId, fileId, cancellationToken);
            return;
        }

        // ── Menu callbacks require authentication ─────────────────────────────
        var existingUser = await userManager.Users
            .FirstOrDefaultAsync(u => u.BaleId == chatId.ToString(), cancellationToken);

        if (existingUser == null)
        {
            await SendMessageAsync(chatId,
                "جلسه شما منقضی شده است. لطفاً /start را ارسال کنید.", cancellationToken);
            return;
        }

        switch (data)
        {
            case MenuFileNumberCallback:
                state.Stage = BaleConversationStage.WaitingForFileNumber;
                await SendMessageAsync(chatId, "شماره پرونده را ارسال کنید:", cancellationToken);
                break;

            case MenuElectRequestCallback:
                state.Stage = BaleConversationStage.WaitingForElectRequestNum;
                await SendMessageAsync(chatId, "شماره درخواست برق را ارسال کنید:", cancellationToken);
                break;

            case MenuEngProjectsCallback:
                await ShowEngineerProjectsAsync(chatId, existingUser, cancellationToken);
                break;

            case MenuBackCallback:
                state.Stage = BaleConversationStage.Authenticated;
                await ShowRoleMenuAsync(chatId, existingUser, cancellationToken);
                break;
        }
    }

    public async Task SendMessageAsync(long chatId, string text, CancellationToken cancellationToken = default)
        => await SendMessageCoreAsync(chatId, text, null, cancellationToken);

    // ── Menus ─────────────────────────────────────────────────────────────────

    private async Task ShowInitialMenuAsync(long chatId, CancellationToken cancellationToken)
    {
        var keyboard = new InlineKeyboardMarkup
        {
            InlineKeyboard =
            [
                [
                    new InlineKeyboardButton { Text = "👤 کاربر",  CallbackData = LoginUserCallback },
                    new InlineKeyboardButton { Text = "🏠 مالک",   CallbackData = LoginLandlordCallback }
                ]
            ]
        };

        await SendMessageCoreAsync(chatId,
            "سلام! به ربات واحد برق خوش آمدید.\n\nلطفاً نوع ورود خود را انتخاب کنید:",
            keyboard, cancellationToken);
    }

    private async Task ShowRoleMenuAsync(long chatId, ApplicationUser user, CancellationToken cancellationToken)
    {
        var roles = await userManager.GetRolesAsync(user);
        var isAdmin    = roles.Any(r => AdminRoles.Contains(r));
        var isEngineer = roles.Contains("Engineer");

        List<List<InlineKeyboardButton>> rows;

        if (isAdmin)
        {
            rows =
            [
                [new InlineKeyboardButton { Text = "🔍 جستجو با شماره پرونده",         CallbackData = MenuFileNumberCallback }],
                [new InlineKeyboardButton { Text = "⚡ جستجو با شماره درخواست برق",    CallbackData = MenuElectRequestCallback }]
            ];
        }
        else if (isEngineer)
        {
            rows =
            [
                [new InlineKeyboardButton { Text = "📋 پرونده‌های من",                 CallbackData = MenuEngProjectsCallback }],
                [new InlineKeyboardButton { Text = "🔍 جستجو با شماره پرونده",         CallbackData = MenuFileNumberCallback }]
            ];
        }
        else
        {
            // Fallback: simple file-number search
            rows =
            [
                [new InlineKeyboardButton { Text = "🔍 جستجو با شماره پرونده",         CallbackData = MenuFileNumberCallback }]
            ];
        }

        await SendMessageCoreAsync(chatId,
            $"خوش آمدید {user.FirstName} {user.LastName}!\n\nلطفاً یک گزینه را انتخاب کنید:",
            new InlineKeyboardMarkup { InlineKeyboard = rows }, cancellationToken);
    }

    private async Task SendBackButtonAsync(long chatId, CancellationToken cancellationToken)
    {
        await SendMessageCoreAsync(chatId, "─────────────",
            new InlineKeyboardMarkup
            {
                InlineKeyboard =
                    [[new InlineKeyboardButton { Text = "🔙 بازگشت به منو", CallbackData = MenuBackCallback }]]
            }, cancellationToken);
    }

    // ── Login ─────────────────────────────────────────────────────────────────

    private async Task HandleLoginAsync(long chatId, BaleConversationState state, string password,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByNameAsync(state.PendingUsername!);

            if (user == null || !user.Active)
            {
                state.PendingUsername = null;
                state.Stage = BaleConversationStage.WaitingForUsername;
                await SendMessageAsync(chatId,
                    "نام کاربری یا رمز عبور اشتباه است.\n\nلطفاً دوباره نام کاربری خود را وارد کنید:",
                    cancellationToken);
                return;
            }

            if (await userManager.IsLockedOutAsync(user))
            {
                stateManager.Reset(chatId);
                await SendMessageAsync(chatId,
                    "حساب کاربری شما موقتاً قفل شده است. لطفاً بعداً تلاش کنید.\n\nبرای شروع مجدد /start را ارسال کنید.",
                    cancellationToken);
                return;
            }

            if (!await userManager.CheckPasswordAsync(user, password))
            {
                await userManager.AccessFailedAsync(user);
                state.PendingUsername = null;
                state.Stage = BaleConversationStage.WaitingForUsername;
                await SendMessageAsync(chatId,
                    "نام کاربری یا رمز عبور اشتباه است.\n\nلطفاً دوباره نام کاربری خود را وارد کنید:",
                    cancellationToken);
                return;
            }

            await userManager.ResetAccessFailedCountAsync(user);
            user.BaleId = chatId.ToString();
            await userManager.UpdateAsync(user);

            state.Stage = BaleConversationStage.Authenticated;
            state.PendingUsername = null;

            await ShowRoleMenuAsync(chatId, user, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during Bale bot login for chatId {ChatId}", chatId);
            stateManager.Reset(chatId);
            await SendMessageAsync(chatId,
                "خطایی رخ داد. لطفاً دوباره تلاش کنید.\n\nبرای شروع مجدد /start را ارسال کنید.",
                cancellationToken);
        }
    }

    // ── Landlord NaCode flow ──────────────────────────────────────────────────

    private async Task HandleLandlordNaCodeInputAsync(long chatId, string naCode,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(naCode))
        {
            await SendMessageAsync(chatId, "لطفاً کد ملی معتبر وارد کنید:", cancellationToken);
            return;
        }

        var projects = await electProjectRepository.GetElectProjectsByLandlordNaCode(naCode);

        if (projects.Count == 0)
        {
            await SendMessageAsync(chatId,
                $"هیچ پرونده‌ای برای کد ملی {naCode} یافت نشد.\n\nبرای شروع مجدد /start را ارسال کنید.",
                cancellationToken);
            stateManager.Reset(chatId);
            return;
        }

        var rows = projects
            .Select(p => new List<InlineKeyboardButton>
            {
                new()
                {
                    Text         = $"📁 پرونده {p.FileNumber} — {p.LandlordName}",
                    CallbackData = $"{ProjectCallbackPrefix}{p.FileNumber}"
                }
            })
            .ToList();

        await SendMessageCoreAsync(chatId,
            $"📋 {projects.Count} پرونده برای کد ملی {naCode} یافت شد.\nبرای مشاهده روی پرونده ضربه بزنید:",
            new InlineKeyboardMarkup { InlineKeyboard = rows }, cancellationToken);

        // Reset so the next /start works cleanly (landlord is not an authenticated user)
        stateManager.Reset(chatId);
    }

    // ── File-number input (authenticated) ─────────────────────────────────────

    private async Task HandleFileNumberInputAsync(long chatId, BaleConversationState state, string text,
        ApplicationUser user, CancellationToken cancellationToken)
    {
        if (!long.TryParse(text, out var fileNumber))
        {
            await SendMessageAsync(chatId,
                "لطفاً یک شماره پرونده معتبر (عدد) ارسال کنید:", cancellationToken);
            return;
        }

        state.Stage = BaleConversationStage.Authenticated;
        await ShowProjectWithFilesAsync(chatId, fileNumber, cancellationToken);
        await SendBackButtonAsync(chatId, cancellationToken);
    }

    // ── ElectRequestNumber input (authenticated – admin roles) ────────────────

    private async Task HandleElectRequestNumInputAsync(long chatId, BaleConversationState state, string text,
        CancellationToken cancellationToken)
    {
        if (!long.TryParse(text, out var electRequestNumber))
        {
            await SendMessageAsync(chatId,
                "لطفاً یک شماره درخواست برق معتبر (عدد) ارسال کنید:", cancellationToken);
            return;
        }

        state.Stage = BaleConversationStage.Authenticated;

        var project = await electProjectRepository.GetElectProjectByElectRequestNumber(electRequestNumber);
        if (project == null)
        {
            await SendMessageAsync(chatId,
                $"پرونده‌ای با شماره درخواست برق {electRequestNumber} یافت نشد.",
                cancellationToken);
        }
        else
        {
            await ShowProjectWithFilesAsync(chatId, project.FileNumber, cancellationToken);
        }

        await SendBackButtonAsync(chatId, cancellationToken);
    }

    // ── Engineer projects list ────────────────────────────────────────────────

    private async Task ShowEngineerProjectsAsync(long chatId, ApplicationUser user,
        CancellationToken cancellationToken)
    {
        try
        {
            var engineer = await engineerRepository.getByUserId(user.Id);
            if (engineer == null)
            {
                await SendMessageAsync(chatId,
                    "❌ اطلاعات مهندسی برای حساب شما یافت نشد.", cancellationToken);
                return;
            }

            var filter = new EppFilterModel(
                searchValue: string.Empty,
                fileNumber: 0,
                solarDateDeliverEngineer: string.Empty,
                landlordName: string.Empty,
                idSection: 0,
                inspectionStatusEnum: InspectionStatusEnum.Undefined,
                page: 0,
                pageSize: 30,
                engineerId: engineer.Id.ToString()
            );

            var result = await electProjectProcessRepository.GetEppByEng(engineer.Id, filter);
            var processes = result.AggregateModel?.ToList() ?? [];

            if (processes.Count == 0)
            {
                await SendMessageAsync(chatId, "📋 پرونده‌ای برای شما ثبت نشده است.", cancellationToken);
                await SendBackButtonAsync(chatId, cancellationToken);
                return;
            }

            // Deduplicate by FileNumber – show each project once
            var distinct = processes
                .Where(p => p.ElectProject != null)
                .GroupBy(p => p.ElectProject.FileNumber)
                .Select(g => g.First())
                .OrderByDescending(p => p.ElectProject.FileNumber)
                .ToList();

            var rows = distinct
                .Select(p => new List<InlineKeyboardButton>
                {
                    new()
                    {
                        Text         = $"📁 پرونده {p.ElectProject.FileNumber} — {p.ElectProject.LandlordName}",
                        CallbackData = $"{ProjectCallbackPrefix}{p.ElectProject.FileNumber}"
                    }
                })
                .ToList();

            rows.Add([new InlineKeyboardButton { Text = "🔙 بازگشت به منو", CallbackData = MenuBackCallback }]);

            await SendMessageCoreAsync(chatId,
                $"📋 {distinct.Count} پرونده برای شما یافت شد. برای مشاهده روی پرونده ضربه بزنید:",
                new InlineKeyboardMarkup { InlineKeyboard = rows }, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching engineer projects for chatId {ChatId}", chatId);
            await SendMessageAsync(chatId,
                "❌ خطا در دریافت پرونده‌ها. لطفاً دوباره تلاش کنید.", cancellationToken);
        }
    }

    // ── Project + files display ───────────────────────────────────────────────

    private async Task ShowProjectWithFilesAsync(long chatId, long fileNumber,
        CancellationToken cancellationToken)
    {
        var project = await electProjectRepository.GetElectProjectByFileNumber(fileNumber);
        if (project == null)
        {
            await SendMessageAsync(chatId,
                $"پرونده‌ای با شماره {fileNumber} یافت نشد.", cancellationToken);
            return;
        }

        await SendMessageAsync(chatId, BuildProjectInfo(project), cancellationToken);

        var files = await electProjectRepository.GetFilesByFileNumber(fileNumber);
        if (files.Count == 0)
        {
            await SendMessageAsync(chatId, "📎 فایلی برای این پرونده ثبت نشده است.", cancellationToken);
            return;
        }

        await SendMessageCoreAsync(chatId,
            "📎 فایل‌های پرونده — برای دانلود روی دکمه مربوطه ضربه بزنید:",
            BuildFileKeyboard(files), cancellationToken);
    }

    // ── File download ─────────────────────────────────────────────────────────

    private async Task DownloadFileAsync(long chatId, Guid fileId, CancellationToken cancellationToken)
    {
        var file = await electProjectFileRepository.GetFileById(fileId);
        if (file == null)
        {
            await SendMessageAsync(chatId, "❌ فایل مورد نظر یافت نشد.", cancellationToken);
            return;
        }

        var s3Key = $"{ElectProjectS3Prefix}/{file.FolderName}/{file.FileName}";
        try
        {
            await SendMessageAsync(chatId, "⏳ در حال دریافت فایل...", cancellationToken);
            var fileStream = await s3Service.GetFullPath(s3Key);
            await SendDocumentAsync(chatId, fileStream, BuildDisplayFileName(file), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch S3 file {Key} for chatId {ChatId}", s3Key, chatId);
            await SendMessageAsync(chatId,
                "❌ خطا در دریافت فایل. لطفاً دوباره تلاش کنید.", cancellationToken);
        }
    }

    // ── Bale API helpers ──────────────────────────────────────────────────────

    private async Task SendMessageCoreAsync(long chatId, string text, InlineKeyboardMarkup? keyboard,
        CancellationToken cancellationToken)
    {
        try
        {
            var client  = httpClientFactory.CreateClient("BaleBot");
            var payload = new BaleSendMessageRequest { ChatId = chatId, Text = text, ReplyMarkup = keyboard };
            var response = await client.PostAsJsonAsync("sendMessage", payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
                logger.LogWarning("Bale sendMessage failed for chatId {ChatId}: {Status}",
                    chatId, response.StatusCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send Bale message to chatId {ChatId}", chatId);
        }
    }

    private async Task AnswerCallbackQueryAsync(string callbackQueryId, CancellationToken cancellationToken)
    {
        try
        {
            var client = httpClientFactory.CreateClient("BaleBot");
            await client.PostAsJsonAsync("answerCallbackQuery",
                new { callback_query_id = callbackQueryId }, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to answer callbackQuery {Id}", callbackQueryId);
        }
    }

    private async Task SendDocumentAsync(long chatId, Stream fileStream, string fileName,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("BaleBot");
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(chatId.ToString()), "chat_id");
        content.Add(new StreamContent(fileStream), "document", fileName);

        var response = await client.PostAsync("sendDocument", content, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogWarning("Bale sendDocument failed for chatId {ChatId}: {Status} – {Body}",
                chatId, response.StatusCode, body);
            throw new InvalidOperationException($"sendDocument failed: {response.StatusCode}");
        }
    }

    // ── Keyboard builder ──────────────────────────────────────────────────────

    /// <summary>
    /// One button row per file. Label = Persian Display name of FileTypeEnum.
    /// callback_data = "file:{guid}" (41 chars — well within the 64-byte limit).
    /// </summary>
    private static InlineKeyboardMarkup BuildFileKeyboard(List<ElectProjectFile> files)
    {
        var rows = files
            .Select(f => new List<InlineKeyboardButton>
            {
                new()
                {
                    Text         = GetFileTypePersianName(f.FileTypeEnum),
                    CallbackData = $"{FileCallbackPrefix}{f.Id}"
                }
            })
            .ToList();

        return new InlineKeyboardMarkup { InlineKeyboard = rows };
    }

    // ── Formatters ────────────────────────────────────────────────────────────

    private static string BuildProjectInfo(ElectProject project)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"📁 شماره پرونده: {project.FileNumber}");

        if (project.ElectRequestNumber > 0)
            sb.AppendLine($"⚡ شماره درخواست برق: {project.ElectRequestNumber}");

        sb.AppendLine($"👤 مالک: {project.LandlordName}");

        if (!string.IsNullOrEmpty(project.LandlordPhoneNumber))
            sb.AppendLine($"📞 تلفن مالک: {project.LandlordPhoneNumber}");

        if (!string.IsNullOrEmpty(project.Address))
            sb.AppendLine($"📍 آدرس: {project.Address}");

        if (!string.IsNullOrEmpty(project.CompanyName))
            sb.AppendLine($"🏢 شرکت: {project.CompanyName}");

        sb.AppendLine($"📐 مساحت: {project.Area} متر مربع");
        sb.AppendLine($"🏗️ تعداد طبقات: {project.NumberOfFloor}");

        if (!string.IsNullOrEmpty(project.SolarRegisterDate))
            sb.AppendLine($"📅 تاریخ ثبت: {project.SolarRegisterDate}");

        sb.AppendLine($"📊 وضعیت: {project.ElectProjectStatusEnum}");
        sb.AppendLine($"🔖 مرحله: {project.ProjectLevelEnum}");
        sb.AppendLine($"✅ تایید شده: {(project.IsOk ? "بله" : "خیر")}");

        if (project.IsStop)
            sb.AppendLine($"⛔ متوقف: {project.StopDes}");

        if (!string.IsNullOrEmpty(project.LicenseNumber))
            sb.AppendLine($"📜 شماره پروانه: {project.LicenseNumber}");

        return sb.ToString();
    }

    private static string BuildDisplayFileName(ElectProjectFile file)
    {
        var ext      = Path.GetExtension(file.FileName);
        var baseName = string.IsNullOrEmpty(file.Name)
            ? Path.GetFileNameWithoutExtension(file.FileName)
            : file.Name;
        return string.IsNullOrEmpty(ext) ? baseName : $"{baseName}{ext}";
    }

    /// <summary>Returns the Persian [Display(Name = "...")] value for a FileTypeEnum member.</summary>
    private static string GetFileTypePersianName(FileTypeEnum fileType)
    {
        var member  = typeof(FileTypeEnum).GetMember(fileType.ToString()).FirstOrDefault();
        var display = member?.GetCustomAttribute<DisplayAttribute>();
        return display?.Name ?? fileType.ToString();
    }
}
