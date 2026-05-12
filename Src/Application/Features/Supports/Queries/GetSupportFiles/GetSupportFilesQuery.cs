using MediatR;
using System.Collections.Generic;

namespace Coreapi.Application.Features.Supports.Queries.GetSupportFiles;

public class GetSupportFilesQuery : IRequest<IEnumerable<SupportFileDto>>
{
    public string SupportId { get; set; }
}