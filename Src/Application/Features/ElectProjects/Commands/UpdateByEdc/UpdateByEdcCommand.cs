using Coreapi.Common.Enums;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.UpdateByEdc;

public class UpdateByEdcCommand:IRequest<int>
{
    public string ElectProjectId { get; set; }
    public string Des { get; set; }
    public bool ErtDefect { get; set; } = false;
    public bool InspectDefect { get; set; } = false;

    public ElectProjectStatusEnum ElectProjectStatusEnum { get; set; }
}