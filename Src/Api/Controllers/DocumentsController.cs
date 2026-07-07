using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.Documents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    [AllowAnonymous]
    public class DocumentsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDocumentsQuery query) =>
            Ok(await Mediator.Send(query));

        [HttpGet]
        public async Task<IActionResult> GetCategories() =>
            Ok(await Mediator.Send(new GetDocumentCategoriesQuery()));

        [HttpPost]
        public async Task<IActionResult> IncrementDownload(IncrementDownloadCommand command)
        {
            var count = await Mediator.Send(command);
            return count is null ? NotFound() : Ok(new { downloadCount = count });
        }
    }
}
