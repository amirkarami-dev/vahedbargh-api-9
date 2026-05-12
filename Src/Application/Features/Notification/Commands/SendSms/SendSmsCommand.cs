using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.Notification.Commands.SendSms
{
    public class SendSmsCommand:IRequest<string>
    {
        public string MobileNumber { get; set; }
        public int TemplateId { get; set; }
        public string[] Params { get; set; }
        public string KeyCode { get; set; }
    }
}
