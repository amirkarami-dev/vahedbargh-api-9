using System;
using System.Collections.Generic;
using Coreapi.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Coreapi.Application.Features.ElectProjects.Commands.AddFile;

public class AddElectProjectFileCommand:IRequest<List<Guid>>
{
    public IFormFile File { get; set; }
    public List<string> ElectProjectId { get; set; }
    public string Name { get; set; }
    public string Des { get; set; }
    public FileTypeEnum FileTypeEnum { get; set; }
    public string FolderName { get; set; }
    public string FileName { get; set; }
    public string UserId { get; set; }
    public string ToUserId { get; set; }
}