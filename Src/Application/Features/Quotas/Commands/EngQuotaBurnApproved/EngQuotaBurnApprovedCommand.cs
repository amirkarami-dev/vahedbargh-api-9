using MediatR;
using System;

namespace Coreapi.Application.Features.Quotas.Commands.EngQuotaBurnApproved;

public class EngQuotaBurnApprovedCommand:IRequest<string>
{
    public Guid Id { get; set; }
}