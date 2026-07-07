using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    [AllowAnonymous]
    public class StatsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await Mediator.Send(new GetStatsQuery()));
    }
}
