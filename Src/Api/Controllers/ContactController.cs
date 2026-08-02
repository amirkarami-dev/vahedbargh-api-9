using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    public class ContactController : BaseController
    {
        // Public: anyone can submit the contact form.
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Send(SendContactFormCommand command) =>
            Ok(await Mediator.Send(command));

        // Admin inbox.
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await Mediator.Send(new GetContactMessagesQuery()));

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> MarkRead(MarkContactReadCommand command) =>
            await Mediator.Send(command) ? Ok() : NotFound();

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteContactMessageCommand command) =>
            await Mediator.Send(command) ? Ok() : NotFound();
    }
}
