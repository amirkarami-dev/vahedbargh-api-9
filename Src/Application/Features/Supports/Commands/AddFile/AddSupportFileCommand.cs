using System;
using Coreapi.Common.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using MediatR;

namespace Coreapi.Application.Features.Supports.Commands.AddFile;

public class AddSupportFileCommand:IRequest<Guid>
{
    public IFormFile File { get; set; }
    public string SupportId { get; set; }
    public string Name { get; set; }
    public string Des { get; set; }
    public FileTypeEnum FileTypeEnum { get; set; }
    public string FolderName { get; set; }
    public string FileName { get; set; }
    public string UserId { get; set; }
    public string ToUserId { get; set; }
}