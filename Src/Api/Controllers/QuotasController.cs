using System.Threading.Tasks;
using Coreapi.Application.Features.Quotas.Commands.EngQuotaBurnApproved;
using Coreapi.Application.Features.Quotas.Commands.EngQuotaBurnUpdate;
using Coreapi.Application.Features.Quotas.Queries.GetEngQuotaBurns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers;

[Authorize]
[Authorize(Roles = "Administrator, Accountant")]
public class QuotasController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetEngQuotaBurnList([FromQuery] GetEngQuotaBurnQuery query) =>
        Ok(await Mediator.Send(query));

    [HttpPost]
    public async Task<IActionResult> EngQuotaBurnApproved([FromBody] EngQuotaBurnApprovedCommand command) =>
        Ok(await Mediator.Send(command));

    [HttpPost]
    public async Task<IActionResult> EngQuotaBurnUpdate([FromBody] EngQuotaBurnUpdateCommand command) =>
        Ok(await Mediator.Send(command));

}