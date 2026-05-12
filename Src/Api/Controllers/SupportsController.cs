using System.Threading.Tasks;
using Coreapi.Api.Attributes;
using Coreapi.Application.Features.Supports.Commands.AddFile;
using Coreapi.Application.Features.Supports.Commands.Closed;
using Coreapi.Application.Features.Supports.Commands.DeleteFile;
using Coreapi.Application.Features.Supports.Commands.Upsert;
using Coreapi.Application.Features.Supports.Commands.UpsertTicket;
using Coreapi.Application.Features.Supports.Queries.GetListSupport;
using Coreapi.Application.Features.Supports.Queries.GetListSupportTicket;
using Coreapi.Application.Features.Supports.Queries.GetSupportFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    [SanitizeInput]
    [Authorize(Roles = "Administrator, SuperUser, Executor, Engineer, Accountant, Employee")]
    public class SupportsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Upsert(UpsertSupportCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> GetSupports([FromQuery] GetListSupportQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> UpsertTicket(UpsertTicketCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> GetTickets([FromQuery] GetListSupportTicketQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        [Authorize(Roles = "Administrator, Accountant, Employee")]
        public async Task<IActionResult> ClosedSupport([FromQuery] ClosedSupportCommand supportCommand) => Ok(await Mediator.Send(supportCommand));

        [HttpPost]
        public async Task<IActionResult> AddFile([FromForm] AddSupportFileCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> GetSupportFiles([FromQuery] GetSupportFilesQuery query) =>
            Ok(await Mediator.Send(query));


        [HttpPost]
        public async Task<IActionResult> DeleteFile(DeleteFileSupportCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
