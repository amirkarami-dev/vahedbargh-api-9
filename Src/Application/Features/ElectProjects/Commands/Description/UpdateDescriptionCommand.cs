using System;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Description;

// Edit only the project's free-text description, from the projects list.
// Deliberately NOT part of UpdateProjectCommand, which replaces the whole record.
public class UpdateDescriptionCommand : IRequest<int>
{
    public Guid ElectProjectId { get; set; }
    public string Description { get; set; }
}
