using MediatR;

namespace Coreapi.Application.Features.Supports.Commands.DeleteFile;

public class DeleteFileSupportCommand: IRequest
{
    public string Id { get; set; }

}