using System;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.AddPanelMaker;

public class AddElectProjectPanelMakerCommand:IRequest<string>
{
public string ElectProjectId { get; set; }
public string PanelMakerId { get; set; }
}