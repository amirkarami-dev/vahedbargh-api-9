using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Delete
{
    public class DeleteProjectsCommand : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
