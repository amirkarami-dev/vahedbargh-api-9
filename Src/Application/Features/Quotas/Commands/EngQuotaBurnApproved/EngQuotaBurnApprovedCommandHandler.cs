using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Quotas.Commands.EngQuotaBurnApproved;

public class EngQuotaBurnApprovedCommandHandler(IEngQuotaBurnRepository engQuotaBurnRepository)
    : IRequestHandler<EngQuotaBurnApprovedCommand, string>
{
    public async Task<string> Handle(EngQuotaBurnApprovedCommand request, CancellationToken cancellationToken)
    {
        var engApproved = await engQuotaBurnRepository.GetById(request.Id);
        if (engApproved == null) throw new NotFoundException("این ردیف پیدا نشد");

        engApproved.OnApproved(!engApproved.Approved);
        await engQuotaBurnRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return engApproved.Id.ToString();
    }
}