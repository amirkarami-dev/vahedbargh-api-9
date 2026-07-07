using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.Announcements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    [AllowAnonymous]
    public class AnnouncementsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllAnnouncementsQuery query) =>
            Ok(await Mediator.Send(query));

        [HttpGet]
        public async Task<IActionResult> GetLatest([FromQuery] GetLatestAnnouncementsQuery query) =>
            Ok(await Mediator.Send(query));

        [HttpGet]
        public async Task<IActionResult> GetUrgent() =>
            Ok(await Mediator.Send(new GetUrgentAnnouncementsQuery()));

        [HttpGet]
        public async Task<IActionResult> GetBySlug([FromQuery] GetAnnouncementBySlugQuery query)
        {
            var result = await Mediator.Send(query);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
