using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using FluentValidation;
using MediatR;

namespace Coreapi.Application.Features.Landing.Contact
{
    public class ContactResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class SendContactFormCommand : IRequest<ContactResultDto>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class SendContactFormCommandValidator : AbstractValidator<SendContactFormCommand>
    {
        public SendContactFormCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Subject).MaximumLength(200);
            RuleFor(x => x.Message).NotEmpty().MaximumLength(4000);
            RuleFor(x => x.Mobile).MaximumLength(20);
            RuleFor(x => x.Email).MaximumLength(200);
        }
    }

    public class SendContactFormCommandHandler(ILandingRepository repo)
        : IRequestHandler<SendContactFormCommand, ContactResultDto>
    {
        public async Task<ContactResultDto> Handle(SendContactFormCommand request, CancellationToken ct)
        {
            await repo.AddContactMessage(new ContactMessage
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Mobile = request.Mobile,
                Subject = request.Subject,
                Message = request.Message,
                CreatedAt = DateTime.UtcNow,
            });

            return new ContactResultDto { Success = true, Message = "پیام شما با موفقیت ثبت شد. به‌زودی با شما تماس خواهیم گرفت." };
        }
    }

    // ---------- Admin inbox ----------

    public class ContactMessageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        public static ContactMessageDto From(ContactMessage m) => new()
        {
            Id = m.Id, Name = m.Name, Email = m.Email, Mobile = m.Mobile, Subject = m.Subject,
            Message = m.Message, CreatedAt = m.CreatedAt, IsRead = m.IsRead,
        };
    }

    public class GetContactMessagesQuery : IRequest<IEnumerable<ContactMessageDto>> { }

    public class MarkContactReadCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public bool IsRead { get; set; } = true;
    }

    public class DeleteContactMessageCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class ContactAdminHandlers(ILandingRepository repo) :
        IRequestHandler<GetContactMessagesQuery, IEnumerable<ContactMessageDto>>,
        IRequestHandler<MarkContactReadCommand, bool>,
        IRequestHandler<DeleteContactMessageCommand, bool>
    {
        public async Task<IEnumerable<ContactMessageDto>> Handle(GetContactMessagesQuery r, CancellationToken ct) =>
            (await repo.GetContactMessages()).Select(ContactMessageDto.From);

        public async Task<bool> Handle(MarkContactReadCommand r, CancellationToken ct) =>
            await repo.MarkContactMessageRead(r.Id, r.IsRead);

        public async Task<bool> Handle(DeleteContactMessageCommand r, CancellationToken ct) =>
            await repo.DeleteContactMessage(r.Id);
    }
}
