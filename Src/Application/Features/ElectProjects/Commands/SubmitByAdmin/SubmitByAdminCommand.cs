using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.SubmitByAdmin
{
    public class SubmitByAdminCommand : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
