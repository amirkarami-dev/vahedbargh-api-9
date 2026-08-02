using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.Meetings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    public class MeetingsController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMeetingsQuery query) =>
            Ok(await Mediator.Send(query));

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLatest([FromQuery] GetLatestMeetingsQuery query) =>
            Ok(await Mediator.Send(query));

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetMeetingByIdQuery query)
        {
            var result = await Mediator.Send(query);
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Upsert(UpsertMeetingCommand command)
        {
            var result = await Mediator.Send(command);
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteMeetingCommand command) =>
            await Mediator.Send(command) ? Ok() : NotFound();
    }
}
