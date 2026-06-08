using System.Threading.Tasks;
using Coreapi.Api.Attributes;
using Coreapi.Application.Features.ElectProjectProcesses.Commands.Accepted;
using Coreapi.Application.Features.ElectProjectProcesses.Commands.Approved;
using Coreapi.Application.Features.ElectProjectProcesses.Commands.ChangeProjectProcess;
using Coreapi.Application.Features.ElectProjectProcesses.Commands.DeleteProjectProcess;
using Coreapi.Application.Features.ElectProjectProcesses.Commands.ProjectProcess;
using Coreapi.Application.Features.ElectProjectProcesses.Commands.UpdateIsMain;
using Coreapi.Application.Features.ElectProjectProcesses.Queries.GetListProjectProcessEng;
using Coreapi.Application.Features.ElectProjectProcesses.Queries.GetProjectProcess;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    [Authorize]
    [SanitizeInput]

    public class ElectProjectProcessesController : BaseController
    {




        [Authorize(Roles = "Administrator, SuperUser, Engineer, Executor, Accountant , Employee, Section")]
        [HttpGet]
        public async Task<IActionResult> GetProjectProcessByEpId([FromQuery] GetProjectProcessQuery query) =>
            Ok(await Mediator.Send(query));

        
        [Authorize(Roles = "Administrator, SuperUser, Section")]
        [HttpPost]
        public async Task<IActionResult> ProjectProcess([FromBody] ProjectProcessCommand command) =>
            Ok(await Mediator.Send(command));

        
        [Authorize(Roles = "Engineer, Administrator, Section")]
        [HttpPost]
        public async Task<IActionResult> GetListProjectProcessEng([FromBody] GetListProjectProcessEngQuery query) =>
            Ok(await Mediator.Send(query));


        [Authorize(Roles = "Administrator, Section")]
        [HttpGet]
        public async Task<IActionResult> DeleteProjectProcess([FromQuery] DeleteProjectProcessCommand query) =>
            Ok(await Mediator.Send(query));


        [Authorize(Roles = "Engineer")]
        [HttpPost]
        public async Task<IActionResult> EppApproved(EppApprovedCommand command) =>
            Ok(await Mediator.Send(command));

        [Authorize(Roles = "Engineer, Administrator")]
        [HttpPost]
        public async Task<IActionResult> EppAccepted(EngAcceptedCommand command) =>
            Ok(await Mediator.Send(command));


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> EppEngChange([FromBody] ChangeProjectProcessCommand command) =>
            Ok(await Mediator.Send(command));


        [Authorize(Roles = "Administrator, SuperUser, Section")]
        [HttpPost]
        public async Task<IActionResult> EppUpdateIsMain([FromBody] UpdateIsMainCommand command) =>
            Ok(await Mediator.Send(command));

    }
}
