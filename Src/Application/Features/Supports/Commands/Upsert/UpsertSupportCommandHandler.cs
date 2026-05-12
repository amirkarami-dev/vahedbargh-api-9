using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using MediatR;

namespace Coreapi.Application.Features.Supports.Commands.Upsert;

public class UpsertSupportCommandHandler(
    IClientRepository clientRepository,
    IUserManager userManager,
    ICurrentUser currentUser,
    ISupportRepository supportRepository,
    ISupportMessageRepository supportMessageRepository,
    ISupportFileRepository supportFileRepository,
    IEngineerRepository engineerRepository,
    IS3Service s3ServiceLiara,
    INotificationService notificationService,
    ISmsService smsService)
    : IRequestHandler<UpsertSupportCommand, string>
{
    public async Task<string> Handle(UpsertSupportCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }
        var fromUser = await userManager.GetUserAsync(currentUser.UserId);
        var toUser = await userManager.GetUserAsync(request.ToUserId);


    

        if (fromUser is null || toUser is null) throw new NotFoundException("کاربری پیدا نشد");

        var ticketNumber = await supportRepository.GetLastTicketNumber(Guid.Parse(currentUser.ClientId)) + 1;
      
            var support = new Support(
                ticketNumber,
                client,
                currentUser.UserId,
                toUser.Id, 
                fromUser.PhoneNumber, 
                fromUser.FirstName,
                toUser.FirstName, 
                UserType.Engineer,
                Helper.MiladiToShamsi(DateTime.Now), 
                DateTime.Now, 
                "", 
                null, 
                request.Title + ":" + request.Message, 
                request.FileNumber, 0,
                false, false, "", ""
                );

            var message = new SupportMessage(
                request.Message, 
                Helper.MiladiToShamsi(DateTime.Now), 
                DateTime.Now,
                fromUser.Id,
                null,
                toUser.Id,
                fromUser.FirstName,
                toUser.FirstName,
                support
                );
            supportMessageRepository.Add(message);

            if (request.File is not null)
            {
                var fileName = request.FileName.Split('.')[0] + Helper.MiladiToShamsiForName(DateTime.Now) + "." + request.FileName.Split('.')[1];
                await s3ServiceLiara.UploadFileAttach(
                    request.File, fileName, request.FolderName, support.Id.ToString());
                var supportFile = new SupportFile(
                    request.Name + "-" + support.Id,
                    request.Des,
                    request.FileTypeEnum,
                    support.Id.ToString(),
                    fileName.Split('.')[0] + "." + fileName.Split('.')[1],
                    currentUser.UserId,
                    request.ToUserId,
                    support
                );
                supportFileRepository.Add(supportFile);
            }

            var unreadMessageCount = await supportRepository.GetCountUnreadMessage(toUser.Id);

            await notificationService.SendMessageNotification(toUser.Id, "یک پیام جدید دارید", unreadMessageCount);
            if (request.SendSms)
                await smsService.SendSms3Params(toUser.PhoneNumber, 15374, request.Title,
                    request.FileNumber == 0 ? "ندارد" : request.FileNumber.ToString(),
                    Helper.MiladiToShamsiForSms(DateTime.Now));

        await supportRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return "Ok";
    }
}