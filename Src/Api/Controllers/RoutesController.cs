using System.Threading.Tasks;
using Coreapi.Application.Features.Routes.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers;

public class RoutesController : BaseController
{
    // GET
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await Mediator.Send(new GetRoutesQuery()));
}