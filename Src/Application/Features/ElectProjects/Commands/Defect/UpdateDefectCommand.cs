using System;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Defect;

public class UpdateDefectCommand:IRequest<int>
{
    public Guid ElectProjectId { get; set; }
    public string DefectDes { get; set; }
}