using System.Threading.Tasks;
using Coreapi.Api.Attributes;
using Coreapi.Application.Features.ElectProjects.Commands.Delete;
using Coreapi.Application.Features.ElectProjects.Commands.Stop;
using Coreapi.Application.Features.ElectProjects.Commands.SubmitByAdmin;
using Coreapi.Application.Features.ElectProjects.Commands.Update;
using Coreapi.Application.Features.ElectProjects.Queries.GetProjectFiles;
using Coreapi.Application.Features.ElectProjects.Queries.GetProjectsFullFilter;
using Coreapi.Application.Features.ElectProjects.Commands.AddFile;
using Coreapi.Application.Features.ElectProjects.Commands.AddPanelMaker;
using Coreapi.Application.Features.ElectProjects.Commands.AmountSms;
using Coreapi.Application.Features.ElectProjects.Commands.Defect;
using Coreapi.Application.Features.ElectProjects.Commands.Status;
using Coreapi.Application.Features.ElectProjects.Commands.SubmitPanel;
using Coreapi.Application.Features.ElectProjects.Commands.UpdateByEdc;
using Coreapi.Application.Features.ElectProjects.Commands.Upsert;
using Coreapi.Application.Features.ElectProjects.Commands.UpsertChecklist;
using Coreapi.Application.Features.ElectProjects.Commands.UpsertComment;
using Coreapi.Application.Features.ElectProjects.Commands.UpsertErtForm;
using Coreapi.Application.Features.ElectProjects.Queries.GetProjectInfo;
using Coreapi.Application.Features.PanelMakers.Queries.GetClientPanelMakers;
using Coreapi.Application.Features.Transactions.Commands.UpdateEngPayment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    [Authorize]
    [SanitizeInput]
    [Authorize(Roles = "Administrator, SuperUser, Executor, Engineer, Accountant, Employee, PanelMaker, ElectAdmin, Section")]
    public class ElectProjectsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Upsert(UpsertElectProjectCommand command) => Ok(await Mediator.Send(command));

        [HttpPost]
        public async Task<IActionResult> UpdateElectProject(UpdateProjectCommand command) => Ok(await Mediator.Send(command));



        [HttpPost]
        public async Task<IActionResult> UpdateElectProjectDetails(UpdateEngPaymentCommand command) => Ok(await Mediator.Send(command));




        [HttpPost]
        public async Task<IActionResult> GetClientElectProjectsFullFilter([FromBody] GetElectProjectsFullFilterQuery query) =>
            Ok(await Mediator.Send(query));




        [HttpPost]
		[AllowAnonymous]

		public async Task<IActionResult> AddFile([FromForm] AddElectProjectFileCommand command) => Ok(await Mediator.Send(command));



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetElectProjectFilesById([FromQuery] GetElectProjectFileQuery query) =>
            Ok(await Mediator.Send(query));



        [HttpPost]
        [Authorize(Roles = "Administrator, Section")]
        public async Task<IActionResult> SubmitByAdmin([FromQuery] SubmitByAdminCommand command) =>
            Ok(await Mediator.Send(command));



        [HttpPost]
        [Authorize(Roles = "Administrator, Section")]
        public async Task<IActionResult> DeleteElectProject([FromBody] DeleteProjectsCommand command) =>
            Ok(await Mediator.Send(command));



        [HttpPost]
        [Authorize(Roles = "Administrator, Section")]
        public async Task<IActionResult> StopElectProject([FromBody] StopProjectCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        [Authorize(Roles = "Engineer")]
        public async Task<IActionResult> UpsertComment([FromBody] UpsertCommentCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        [Authorize(Roles = "Engineer")]
        public async Task<IActionResult> UpsertCheckList([FromBody] UpsertCheckListCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        public async Task<IActionResult> UpsertCheckListEdc([FromBody] UpsertCheckListEdcCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        [Authorize(Roles = "Engineer")]
        public async Task<IActionResult> UpsertErtForm([FromBody] UpsertErtFormCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> GetPanelMaker() =>
            Ok(await Mediator.Send(new GetClientPanelMakerQuery()));

        [HttpPost]
        public async Task<IActionResult> AddPanelMaker([FromBody] AddElectProjectPanelMakerCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        [Authorize(Roles = "PanelMaker")]
        public async Task<IActionResult> SubmitPanel([FromBody] SubmitPanelCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        public async Task<IActionResult> AmountSms([FromBody] AmountSmsCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        public async Task<IActionResult> UpdateElectProjectStatus([FromBody] UpdateElectProjectStatusCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        public async Task<IActionResult> UpdateDefectDes([FromBody] UpdateDefectCommand command) =>
            Ok(await Mediator.Send(command));


        [HttpPost]
        public async Task<IActionResult> UpdateByEdc([FromBody] UpdateByEdcCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetProjectInfo([FromQuery] GetProjectInfoQuery common) => Ok(await Mediator.Send(common));


    }
}
