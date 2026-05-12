using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using MediatR;

namespace Coreapi.Application.Features.Notification.Commands.SendSms
{       
    public class SendSmsCommandHandler:IRequestHandler<SendSmsCommand,string>
    {
        private readonly ISmsService _smsService;

        public SendSmsCommandHandler(ISmsService smsService)
        {
            _smsService = smsService;
        }
        public async Task<string> Handle(SendSmsCommand request, CancellationToken cancellationToken)
        {
           var result = await _smsService.SendSmsCodeParams(request.MobileNumber, request.TemplateId, request.Params,request.KeyCode);
           return result.Status;
        }
    }
}
