using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.Documents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    public class DocumentsController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDocumentsQuery query) =>
            Ok(await Mediator.Send(query));

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategories() =>
            Ok(await Mediator.Send(new GetDocumentCategoriesQuery()));

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> IncrementDownload(IncrementDownloadCommand command)
        {
            var count = await Mediator.Send(command);
            return count is null ? NotFound() : Ok(new { downloadCount = count });
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Upsert(UpsertDocumentCommand command)
        {
            var result = await Mediator.Send(command);
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteDocumentCommand command) =>
            await Mediator.Send(command) ? Ok() : NotFound();
    }
}
