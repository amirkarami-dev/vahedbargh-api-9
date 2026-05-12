using Coreapi.Common.Enums;
using MediatR;
using System;

namespace Coreapi.Application.Features.ElectProjects.Commands.UpsertComment;

public class UpsertCommentCommand:IRequest<string>
{
    public string Id { get; set; }
    public string ElectProjectId { get; set; }
    public string EppId { get; set; }
    public BranchingTypeEnum BranchingTypeEnum { get; set; }
    public FazNumberEnum FazNumberEnum { get; set; }
    public int BranchingCount { get; set; }
    public float Ampere { get; set; }
    public float Power { get; set; }
    public float PowerSum { get; set; }
    public string Des { get; set; }
    public string DeleteId { get; set; }
}