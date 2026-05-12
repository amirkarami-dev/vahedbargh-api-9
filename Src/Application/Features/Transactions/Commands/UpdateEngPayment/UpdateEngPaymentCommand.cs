using MediatR;
using System.ComponentModel;
using Coreapi.Domain.AggregatesModel.FinanceAgg;

namespace Coreapi.Application.Features.Transactions.Commands.UpdateEngPayment;

public class UpdateEngPaymentCommand:IRequest<EngPaymentList>
{
    public string Id { get; set; }
    public long AmountSystem { get; set; }
    [Description("this is Insurance Deduction ")]
    public long Deduction2 { get; set; }
    [Description("this is Account Deduction ")]
    public long Deduction3 { get; set; }
    [Description("this is Other Deduction ")]
    public long Deduction4 { get; set; }
    [Description("this is fish Addition")]
    public long Addition1 { get; set; }
    [Description("this is Other Addition")]
    public long Addition2 { get; set; }

    public bool PayByBankReceipt { get; set; }
}