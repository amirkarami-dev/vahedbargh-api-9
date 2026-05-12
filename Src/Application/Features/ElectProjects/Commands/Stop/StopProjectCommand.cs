using System;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Stop;

public class StopProjectCommand : IRequest<int>
{
    public Guid GpId { get; set; }
    public string StopDes { get; set; }
    public bool IsStop { get; set; }

}