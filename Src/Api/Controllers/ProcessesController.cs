using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.Processes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    public class ProcessesController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await Mediator.Send(new GetProcessFlowsQuery()));

        // Admin management (Administrator role).
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetAllAdmin() => Ok(await Mediator.Send(new GetProcessFlowsAdminQuery()));

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Upsert(UpsertProcessFlowCommand command)
        {
            var result = await Mediator.Send(command);
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteProcessFlowCommand command) =>
            await Mediator.Send(command) ? Ok() : NotFound();
    }
}
