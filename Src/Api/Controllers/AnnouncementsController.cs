using System;
using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.Announcements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    // Public reads are [AllowAnonymous]; admin writes require the Administrator role.
    public class AnnouncementsController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllAnnouncementsQuery query) =>
            Ok(await Mediator.Send(query));

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLatest([FromQuery] GetLatestAnnouncementsQuery query) =>
            Ok(await Mediator.Send(query));

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUrgent() =>
            Ok(await Mediator.Send(new GetUrgentAnnouncementsQuery()));

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBySlug([FromQuery] GetAnnouncementBySlugQuery query)
        {
            var result = await Mediator.Send(query);
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Upsert(UpsertAnnouncementCommand command)
        {
            var result = await Mediator.Send(command);
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteAnnouncementCommand command) =>
            await Mediator.Send(command) ? Ok() : NotFound();
    }
}
