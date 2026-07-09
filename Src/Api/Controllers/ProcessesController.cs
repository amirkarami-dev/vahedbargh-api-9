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
    }
}
