using System.Threading.Tasks;
using Coreapi.Application.Features.QuarterTariffs.Queries.GetListQuarterTariff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{

    [Authorize]
    public class QuarterTariffsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetQuarterTariffs() =>
            Ok(await Mediator.Send(new GetListQuarterTariffQuery()));
    }
}
