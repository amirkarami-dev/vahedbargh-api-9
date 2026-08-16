using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using FluentValidation;
using MediatR;

namespace Coreapi.Application.Features.Landing.ExpertRequests
{
    // «فرم درخواست کارشناس» — anonymous submission from the public site, plus the
    // Administrator inbox that reads it. Mirrors the Contact feature's shape.

    public class ExpertRequestResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class SubmitExpertRequestCommand : IRequest<ExpertRequestResultDto>
    {
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string NaCode { get; set; }
    }

    public static class NationalCode
    {
        /// <summary>
        /// Iranian national code (کد ملی) check-digit validation. Ten digits, where the last is a
        /// checksum over the first nine. Repdigit codes such as 1111111111 satisfy the arithmetic
        /// but are not issued, so they are rejected explicitly.
        /// </summary>
        public static bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (value.Length != 10 || !value.All(char.IsDigit)) return false;
            if (value.Distinct().Count() == 1) return false;

            var sum = 0;
            for (var i = 0; i < 9; i++) sum += (value[i] - '0') * (10 - i);

            var remainder = sum % 11;
            var check = value[9] - '0';
            return remainder < 2 ? check == remainder : check == 11 - remainder;
        }
    }

    public class SubmitExpertRequestCommandValidator : AbstractValidator<SubmitExpertRequestCommand>
    {
        public SubmitExpertRequestCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("نام و نام خانوادگی را وارد کنید.")
                .MaximumLength(120).WithMessage("نام و نام خانوادگی حداکثر ۱۲۰ نویسه است.");

            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("شماره موبایل را وارد کنید.")
                .Matches(@"^09\d{9}$").WithMessage("شماره موبایل باید ۱۱ رقم و با ۰۹ شروع شود.");

            RuleFor(x => x.NaCode)
                .NotEmpty().WithMessage("کد ملی را وارد کنید.")
                .Must(NationalCode.IsValid).WithMessage("کد ملی معتبر نیست.");
        }
    }

    public class SubmitExpertRequestCommandHandler(ILandingRepository repo, ISmsService smsService)
        : IRequestHandler<SubmitExpertRequestCommand, ExpertRequestResultDto>
    {
        public async Task<ExpertRequestResultDto> Handle(SubmitExpertRequestCommand request, CancellationToken ct)
        {
            var fullName = request.FullName.Trim();
            var mobile = request.MobileNumber.Trim();

            await repo.AddExpertRequest(new ExpertRequest
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                MobileNumber = mobile,
                NaCode = request.NaCode.Trim(),
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
            });

            // The request is already persisted, so a failing SMS gateway must not turn a saved
            // request into an error for the applicant. Template 7396 is the generic 4-param
            // notification template already used by RequestDemo.
            try
            {
                await smsService.SendSms4Params(
                    mobile, 7396,
                    "درخواست کارشناس ثبت شد",
                    fullName,
                    "دفتر اجرایی نظارت برق به‌زودی با شما تماس می‌گیرد",
                    Helper.MiladiToShamsiForSms(DateTime.UtcNow));
            }
            catch
            {
                // Swallowed deliberately — see above.
            }

            return new ExpertRequestResultDto
            {
                Success = true,
                Message = "درخواست شما با موفقیت ثبت شد. نتیجه از طریق پیامک به شما اطلاع داده می‌شود.",
            };
        }
    }

    // ---------- Admin inbox ----------

    public class ExpertRequestDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string NaCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        public static ExpertRequestDto From(ExpertRequest r) => new()
        {
            Id = r.Id, FullName = r.FullName, MobileNumber = r.MobileNumber,
            NaCode = r.NaCode, CreatedAt = r.CreatedAt, IsRead = r.IsRead,
        };
    }

    public class GetExpertRequestsQuery : IRequest<IEnumerable<ExpertRequestDto>> { }

    public class MarkExpertRequestReadCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public bool IsRead { get; set; } = true;
    }

    public class DeleteExpertRequestCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class ExpertRequestAdminHandlers(ILandingRepository repo) :
        IRequestHandler<GetExpertRequestsQuery, IEnumerable<ExpertRequestDto>>,
        IRequestHandler<MarkExpertRequestReadCommand, bool>,
        IRequestHandler<DeleteExpertRequestCommand, bool>
    {
        public async Task<IEnumerable<ExpertRequestDto>> Handle(GetExpertRequestsQuery r, CancellationToken ct) =>
            (await repo.GetExpertRequests()).Select(ExpertRequestDto.From);

        public async Task<bool> Handle(MarkExpertRequestReadCommand r, CancellationToken ct) =>
            await repo.MarkExpertRequestRead(r.Id, r.IsRead);

        public async Task<bool> Handle(DeleteExpertRequestCommand r, CancellationToken ct) =>
            await repo.DeleteExpertRequest(r.Id);
    }
}
