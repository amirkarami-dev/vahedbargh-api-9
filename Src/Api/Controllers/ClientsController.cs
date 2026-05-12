using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Coreapi.Application.Features.Clients.Commands.AddUser;
using Coreapi.Application.Features.Clients.Commands.AddUsers;
using Coreapi.Application.Features.Clients.Commands.SignupClient;
using Coreapi.Application.Features.Clients.Commands.UpdateUser;
using Coreapi.Application.Features.Clients.Queries.IsExist;
using Coreapi.Application.Features.Clients.Queries.GetClientUsers;
using Coreapi.Application.Features.Clients.Commands.DeleteUser;

namespace Coreapi.Api.Controllers
{
    
    [Authorize]
    public class ClientsController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(SignupClientCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IsExist([FromQuery] IsSubdomainExistQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        //[PackageCheck]
        public async Task<IActionResult> AddUser(AddClientUserCommand command) => Ok(await Mediator.Send(command));

   

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        //[PackageCheck]
        public async Task<IActionResult> AddUsers(AddClientUsersCommand command) => Ok(await Mediator.Send(command));



        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateUser(UpdateClientUserCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(DeleteClientUserCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetClientAllUsersQuery query) => Ok(await Mediator.Send(query));

    }
}
