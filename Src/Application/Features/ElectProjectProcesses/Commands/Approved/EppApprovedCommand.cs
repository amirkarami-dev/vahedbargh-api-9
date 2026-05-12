using Coreapi.Common.Enums;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.Approved;

public class EppApprovedCommand:IRequest<string>
{
    public string EppId { get; set; }
    public string Des { get; set; }
    public bool NeedElectNetwork { get; set; }
    public InspectionStatusEnum InspectionStatusEnum { get; set; }
    
}