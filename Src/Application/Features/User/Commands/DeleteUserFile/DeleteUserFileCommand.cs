using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.User.Commands.DeleteUserFile
{
    public class DeleteUserFileCommand:IRequest
    {
        public Guid Id { get; set; }
    }
}
