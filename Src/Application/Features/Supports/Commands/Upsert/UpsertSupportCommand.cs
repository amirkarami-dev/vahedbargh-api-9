using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace Coreapi.Application.Features.Supports.Commands.Upsert;

public class UpsertSupportCommand:IRequest<string>
{

    public string Title { get; set; }
    public string Message { get; set; }
    public long FileNumber { get; set; } = 0;
    public string ToRole { get; set; }

    public IFormFile File { get; set; }
    public string Name { get; set; }
    public string Des { get; set; }
    public FileTypeEnum FileTypeEnum { get; set; }
    public string FolderName { get; set; }
    public string FileName { get; set; }
    public string UserId { get; set; }
    public string ToUserId { get; set; }
    public bool SendSms { get; set; } = false;

}