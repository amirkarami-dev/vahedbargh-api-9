using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using MediatR;
using System;

namespace Coreapi.Application.Features.Engineers.Commands.UpsertEngHistory;

public class UpsertEngHistoryCommand:IRequest<int>
{
    public string EngId { get; set; }
    public string Id { get; init; }
    public EngineerGradeTypeEnum EngineerGradeTypeEnum { get; set; }
    public long WorkPermitNum { get; set; }
    public string SolarIssueDate { get; set; }
    public string SolarValidityDate { get; set; }
    public bool WorkPermission { get; set; }
    public WorkPermitTypeEnum WorkPermitTypeEnum { get; set; }
}