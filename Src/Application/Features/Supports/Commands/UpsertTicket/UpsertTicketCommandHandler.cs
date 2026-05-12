using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using MediatR;

namespace Coreapi.Application.Features.Supports.Commands.UpsertTicket;

public class UpsertTicketCommandHandler(
    IClientRepository clientRepository,
    IUserManager userManager,
    ICurrentUser currentUser,
    ISupportRepository supportRepository,
    ISupportMessageRepository supportMessageRepository)
    : IRequestHandler<UpsertTicketCommand, string>
{
    public async Task<string> Handle(UpsertTicketCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }

        var support = await supportRepository.GetById(Guid.Parse(request.SupportId));
        if (support == null) throw new NotFoundException("این تسکت پشتیبانی وجود ندارد");

        if (support.Closed) throw new NotFoundException("این تیکت قبلا بسته شده است");

        var fromUser = await userManager.GetUserAsync(currentUser.UserId);

        var toUser =  await userManager.GetUserAsync(support.ToUserId);
        
        // اینجا کاربر جاری و تو یوزر یکی باشه یعنی وقتی تیکت ایجادشده برای من ارسال شده و من میخوام بهش پاسخ بدم
        if(toUser.Id == fromUser.Id)
            toUser = await userManager.GetUserAsync(support.UserId);


        if (fromUser is null || toUser is null) throw new NotFoundException("کاربری پیدا نشد");


        var message = new SupportMessage(
            request.Message, 
            Helper.MiladiToShamsiForSupport(DateTime.Now), 
            DateTime.Now,
            fromUser.Id,
            request.FileName, 
            toUser.Id, 
            fromUser.FirstName,
            toUser.LastName, 
            support);
        support.UpdateRead(false);

        supportMessageRepository.Add(message);
        await supportMessageRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return message.Id.ToString();
    }
}