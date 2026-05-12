using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.Accepted;

public class EngAcceptedCommand:IRequest<string>
{
    public string EppId { get; set; }

}