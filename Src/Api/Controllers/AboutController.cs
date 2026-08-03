using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.About;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    public class AboutController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new GetAboutQuery());
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Upsert(UpsertAboutCommand command)
        {
            var result = await Mediator.Send(command);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
