using Coreapi.Common.Enums;
using MediatR;
using System;

namespace Coreapi.Application.Features.ElectProjects.Commands.UpsertChecklist;

public class UpsertCheckListEdcCommand:IRequest<string>
{
    public string Id { get; set; }
    public string ElectProjectId { get; set; }
    public string EppId { get; set; }

    public string SolarChecked { get; set; }
    public DateTime JulianChecked { get; set; }
    public CheckListEdcEnum CheckListEdcEnum { get; set; }
    public bool? IsComplete { get; set; }
    public string ResultDes { get; set; }
    public string DeleteId { get; set; }
}