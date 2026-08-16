using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.ExpertRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    public class ExpertRequestsController : BaseController
    {
        // Public: anyone can submit «فرم درخواست کارشناس» from the landing site.
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Send(SubmitExpertRequestCommand command) =>
            Ok(await Mediator.Send(command));

        // Admin inbox.
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await Mediator.Send(new GetExpertRequestsQuery()));

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> MarkRead(MarkExpertRequestReadCommand command) =>
            await Mediator.Send(command) ? Ok() : NotFound();

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteExpertRequestCommand command) =>
            await Mediator.Send(command) ? Ok() : NotFound();
    }
}
