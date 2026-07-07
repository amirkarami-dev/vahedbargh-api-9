using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.Meetings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    [AllowAnonymous]
    public class MeetingsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMeetingsQuery query) =>
            Ok(await Mediator.Send(query));

        [HttpGet]
        public async Task<IActionResult> GetLatest([FromQuery] GetLatestMeetingsQuery query) =>
            Ok(await Mediator.Send(query));

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetMeetingByIdQuery query)
        {
            var result = await Mediator.Send(query);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
