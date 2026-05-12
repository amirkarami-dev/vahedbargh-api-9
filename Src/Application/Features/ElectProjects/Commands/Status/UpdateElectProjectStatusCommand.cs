using System;
using Coreapi.Common.Enums;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Status;

public class UpdateElectProjectStatusCommand:IRequest<int>
{
    public Guid ElectProjectId { get; set; }
    public ElectProjectStatusEnum ElectProjectStatusEnum { get; set; }
}