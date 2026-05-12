using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.SubmitPanel;

public class SubmitPanelCommand:IRequest<string>
{
    public string Id { get; set; }
    public string PanelSerialNumber { get; set; }
}