using System.Threading.Tasks;
using Coreapi.Application.Features.Engineers.Commands.Upsert;
using Coreapi.Application.Features.Engineers.Commands.UpsertEngHistory;
using Coreapi.Application.Features.Engineers.Queries.GetListEngineer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{

    [Authorize]
    public class EngineersController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetEngineerByClient([FromQuery] GetListEngineerQuery query) =>
            Ok(await Mediator.Send(query)); 

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpsertEngineer([FromBody] UpsertEngineerCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpsertEngHistory([FromBody] UpsertEngHistoryCommand command) =>
            Ok(await Mediator.Send(command));
    }
}
