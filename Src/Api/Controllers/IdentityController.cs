using System.Threading.Tasks;
using Coreapi.Api.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Coreapi.Application.Features.Identity.Commands.ChangePassword;
using Coreapi.Application.Features.Identity.Commands.Login;
using Coreapi.Application.Features.Identity.Commands.LoginByCode;
using Coreapi.Application.Features.Identity.Commands.LoginByGoogle;
using Coreapi.Application.Features.Identity.Commands.LoginByMac;
using Coreapi.Application.Features.Identity.Commands.LoginByMicrosoft;
using Coreapi.Application.Features.Identity.Commands.Logout;
using Coreapi.Application.Features.Identity.Commands.RefreshToken;
using Coreapi.Application.Features.Identity.Commands.ResetToDefaultPassword;
using Coreapi.Application.Features.Transactions.Commands.PaymentMelliPublic;
using Coreapi.Application.Features.User.Commands.ResetPassword;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Coreapi.Api.Controllers
{
    public class IdentityController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand command) => Ok(await Mediator.Send(command));

        [HttpPost]
        public async Task<IActionResult> LoginByCode(LoginByCodeCommand command) => Ok(await Mediator.Send(command));


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await Mediator.Send(new LogoutCommand());
            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> ChangeToDefaultPassword(ResetToDefaultPasswordCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        public async Task<IActionResult> PaymentMelliPublic([FromBody] PaymentMelliPublicCommand command) => Ok(await Mediator.Send(command));

    }
}
