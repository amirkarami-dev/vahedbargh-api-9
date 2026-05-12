using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Features.Notification.Commands.SendSms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{

    public class NotificationController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> SendSms(SendSmsCommand command) => Ok(await Mediator.Send(command));

    }
}
