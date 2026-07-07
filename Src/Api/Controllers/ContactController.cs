using System.Threading.Tasks;
using Coreapi.Application.Features.Landing.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{
    [AllowAnonymous]
    public class ContactController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Send(SendContactFormCommand command) =>
            Ok(await Mediator.Send(command));
    }
}
