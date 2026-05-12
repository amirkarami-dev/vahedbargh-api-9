using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Utility;
using MediatR;

namespace Coreapi.Application.Features.Notification.Commands.RequestDemo
{
    public class RequestDemoCommandHandler(ISmsService smsService) : IRequestHandler<RequestDemoCommand, string>
    {
        public async Task<string> Handle(RequestDemoCommand request, CancellationToken cancellationToken)
        {
            // send to user
            await smsService.SendSms4Params(request.Mobile, 7396, "درخواست دمو ثبت شد"," به زودی با شما تماس خواهیم گرفت ", " با تشکر:کرمی 09120833933", Helper.MiladiToShamsiForSms(DateTime.Now));

            // send internal notification
            await smsService.SendSms4Params("09120833933", 7396, "یک درخواست دمو ثبت شد", request.Mobile , request.Name, Helper.MiladiToShamsiForSms(DateTime.Now));

            return "درخواست شما ثبت شد";
        }
    }
}
