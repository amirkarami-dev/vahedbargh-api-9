using System;
using MediatR;

namespace Coreapi.Application.Features.Quotas.Commands.EngQuotaBurnUpdate;

public class EngQuotaBurnUpdateCommand:IRequest<string>
{
    public string Id { get; set; }
    public string EngId { get; set; }
    public string QtId { get; set; }
    public long AmountBurning { get; set; }
    public int ErtCountBurning { get; set; }
    public double InspectionDelayFactor { get; set; }
    public double ErtDelayFactor { get; set; }
    public string Des { get; set; }
}