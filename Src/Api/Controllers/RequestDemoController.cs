using System.Threading.Tasks;
using Coreapi.Application.Features.Notification.Commands.RequestDemo;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    public class RequestDemoController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> RequestDemo(RequestDemoCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
