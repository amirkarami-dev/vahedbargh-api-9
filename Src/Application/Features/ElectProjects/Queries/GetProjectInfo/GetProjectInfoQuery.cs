using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Queries.GetProjectInfo;

public class GetProjectInfoQuery:IRequest<ProjectInfoDto>
{
    public string ProjectId { get; set; }
}