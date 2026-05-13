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
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
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
    ILogger<BaleService> logger) : IBaleService
{
    // S3 folder prefix used by ElectProject file uploads (see AddElectProjectFileCommandHandler)
    private const string ElectProjectS3Prefix = "Upload/electProjects";
    private const string FileCallbackPrefix = "file:";

    // ── Public entry points ───────────────────────────────────────────────────

    public async Task ProcessUpdateAsync(long chatId, string text, CancellationToken cancellationToken = default)
    {
        text = text?.Trim() ?? string.Empty;

        var existingUser = await userManager.Users
            .FirstOrDefaultAsync(u => u.BaleId == chatId.ToString(), cancellationToken);

        // Already authenticated
        if (existingUser != null)
        {
            var state = stateManager.GetOrCreate(chatId);

            if (text == "/start")
            {
                state.Stage = BaleConversationStage.Authenticated;
                await SendMessageAsync(chatId, "خوش آمدید!\n\nشماره پرونده را ارسال کنید:", cancellationToken);
                return;
            }

            // Normalise in case bot restarted and state was lost
            if (state.Stage is BaleConversationStage.WaitingForUsername
                             or BaleConversationStage.WaitingForPassword)
            {
                state.Stage = BaleConversationStage.Authenticated;
                state.PendingUsername = null;
            }

            await HandleProjectQueryAsync(chatId, text, cancellationToken);
            return;
        }

        // Not yet authenticated – login flow
        if (text == "/start")
            stateManager.Reset(chatId);

        var loginState = stateManager.GetOrCreate(chatId);

        switch (loginState.Stage)
        {
            case BaleConversationStage.WaitingForUsername:
                if (text == "/start" || string.IsNullOrEmpty(text))
                    await SendMessageAsync(chatId,
                        "سلام! به ربات واحد برق خوش آمدید.\n\nلطفاً نام کاربری خود را وارد کنید:",
                        cancellationToken);
                else
                {
                    loginState.PendingUsername = text;
                    loginState.Stage = BaleConversationStage.WaitingForPassword;
                    await SendMessageAsync(chatId, "لطفاً رمز عبور خود را وارد کنید:", cancellationToken);
                }
                break;

            case BaleConversationStage.WaitingForPassword:
                await HandleLoginAsync(chatId, loginState, text, cancellationToken);
                break;
        }
    }

    public async Task HandleCallbackQueryAsync(long chatId, string callbackQueryId, string data,
        CancellationToken cancellationToken = default)
    {
        // Always answer immediately to dismiss the loading indicator on the button
        await AnswerCallbackQueryAsync(callbackQueryId, cancellationToken);

        if (!data.StartsWith(FileCallbackPrefix))
            return;

        if (!Guid.TryParse(data[FileCallbackPrefix.Length..], out var fileId))
            return;

        // Re-verify the user is still authenticated
        var existingUser = await userManager.Users
            .FirstOrDefaultAsync(u => u.BaleId == chatId.ToString(), cancellationToken);

        if (existingUser == null)
        {
            await SendMessageAsync(chatId,
                "جلسه شما منقضی شده است. لطفاً دوباره /start را ارسال کنید.", cancellationToken);
            return;
        }

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
            await SendMessageAsync(chatId, "❌ خطا در دریافت فایل. لطفاً دوباره تلاش کنید.", cancellationToken);
        }
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

            await SendMessageAsync(chatId,
                $"ورود موفق! خوش آمدید {user.FirstName} {user.LastName}.\n\nشماره پرونده را ارسال کنید:",
                cancellationToken);
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

    // ── Project query ─────────────────────────────────────────────────────────

    private async Task HandleProjectQueryAsync(long chatId, string text, CancellationToken cancellationToken)
    {
        if (!long.TryParse(text, out var fileNumber))
        {
            await SendMessageAsync(chatId,
                "لطفاً یک شماره پرونده معتبر (عدد) ارسال کنید:", cancellationToken);
            return;
        }

        var project = await electProjectRepository.GetElectProjectByFileNumber(fileNumber);
        if (project == null)
        {
            await SendMessageAsync(chatId, $"پرونده‌ای با شماره {fileNumber} یافت نشد.", cancellationToken);
            return;
        }

        // Send project summary
        await SendMessageAsync(chatId, BuildProjectInfo(project), cancellationToken);

        // Send file buttons (or a "no files" notice)
        var files = await electProjectRepository.GetFilesByFileNumber(fileNumber);
        if (files.Count == 0)
        {
            await SendMessageAsync(chatId, "📎 فایلی برای این پرونده ثبت نشده است.", cancellationToken);
            return;
        }

        var keyboard = BuildFileKeyboard(files);
        await SendMessageAsync(chatId, "📎 فایل‌های پرونده — برای دانلود روی دکمه مربوطه ضربه بزنید:",
            keyboard, cancellationToken);
    }

    // ── Bale API helpers ──────────────────────────────────────────────────────

    public async Task SendMessageAsync(long chatId, string text, CancellationToken cancellationToken = default)
        => await SendMessageAsync(chatId, text, null, cancellationToken);

    private async Task SendMessageAsync(long chatId, string text, InlineKeyboardMarkup? keyboard,
        CancellationToken cancellationToken)
    {
        try
        {
            var client = httpClientFactory.CreateClient("BaleBot");
            var payload = new BaleSendMessageRequest { ChatId = chatId, Text = text, ReplyMarkup = keyboard };
            var response = await client.PostAsJsonAsync("sendMessage", payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
                logger.LogWarning("Bale sendMessage failed for chatId {ChatId}: {Status}", chatId, response.StatusCode);
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
    /// Builds one button row per file. Button label = Persian display name of FileTypeEnum.
    /// callback_data = "file:{guid}" (41 chars max, well within the 64-byte limit).
    /// </summary>
    private static InlineKeyboardMarkup BuildFileKeyboard(List<ElectProjectFile> files)
    {
        var rows = files.Select(f => new List<InlineKeyboardButton>
        {
            new()
            {
                Text = GetFileTypePersianName(f.FileTypeEnum),
                CallbackData = $"{FileCallbackPrefix}{f.Id}"
            }
        }).ToList();

        return new InlineKeyboardMarkup { InlineKeyboard = rows };
    }

    // ── Formatters ────────────────────────────────────────────────────────────

    private static string BuildProjectInfo(ElectProject project)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"📁 شماره پرونده: {project.FileNumber}");

        if (project.ElectRequestNumber > 0)
            sb.AppendLine($"🔢 شماره درخواست برق: {project.ElectRequestNumber}");

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
        var ext = Path.GetExtension(file.FileName);
        var baseName = string.IsNullOrEmpty(file.Name)
            ? Path.GetFileNameWithoutExtension(file.FileName)
            : file.Name;
        return string.IsNullOrEmpty(ext) ? baseName : $"{baseName}{ext}";
    }

    /// <summary>Returns the Persian [Display(Name = "...")] value for a FileTypeEnum member.</summary>
    private static string GetFileTypePersianName(FileTypeEnum fileType)
    {
        var member = typeof(FileTypeEnum)
            .GetMember(fileType.ToString())
            .FirstOrDefault();

        var display = member?.GetCustomAttribute<DisplayAttribute>();
        return display?.Name ?? fileType.ToString();
    }
}
