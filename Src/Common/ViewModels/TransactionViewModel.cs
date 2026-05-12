using Coreapi.Common.Enums;
using System;

namespace Coreapi.Common.ViewModels;

public class TransactionViewModel
{
    public Guid Id { get; init; }
    public long Amount { get; set; }
    public string UserId { get; set; }
    public string ProjectId { get; set; }
    public GatewayTypeEnum GatewayType { get; init; }
    public string GatewayTypeName { get; set; }
    public TransactionTypeEnum TransactionType { get; set; }
    public string TransactionTypeName { get; set; }
    public TransactionStatusEnum TransactionStatus { get; init; }
    public string TransactionStatusName { get; init; }
    public DateTime JulianCreated { get; init; }
    public string SolarCreated { get; init; }
    public string BankTransactionId { get; set; }
    public string Des { get; set; }
    public string FileNumber { get; set; }
    public int IdCity { get; set; }
    public int IdSection { get; set; }
}