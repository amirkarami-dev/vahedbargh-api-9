using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Transactions;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg;

public class EngPaymentList
{
    private EngPaymentList() { }

    public EngPaymentList(Engineer engineer, EngPaymentTask engPaymentTask, long amountSystem, bool payByBankReceipt)
    {
        Id = Guid.NewGuid();
        Engineer = engineer;
        EngPaymentTask = engPaymentTask;
        AmountSystem = amountSystem;
        Deduction1 = (amountSystem * 5) / 100;
        Deduction2 = engineer.Has1Percent? (amountSystem * 1) / 100:0;
        Deduction3 = (amountSystem * 7) / 100;
        Deduction4 = (Deduction1 * 10) / 100;
        SumAmountSystem = amountSystem - Deduction1 -Deduction2 - Deduction3 -Deduction4;
        SumAmountWithFish = amountSystem - Deduction1 - Deduction2 - Deduction3 - Deduction4;
        PayByBankReceipt = payByBankReceipt;
    }



    public Guid Id { get; init; }
    public Engineer Engineer { get; set; }
    public EngPaymentTask EngPaymentTask { get; set; }
    [Description("this is amount result of system")]
    public long AmountSystem { get; set; }

    [Description("deduction 5 %")]
    public long Deduction1 { get; set; }

    [Description("Deduction 1% sandogh ")]
    public long Deduction2 { get; set; }

    [Description("Deduction 7% vahed bargh")]
    public long Deduction3 { get; set; }

    [Description("Deduction 10% afzodeh ")]
    public long Deduction4 { get; set; }

    [Description("this is fish Addition")]
    public long Addition1 { get; set; }
    [Description("this is Other Addition")]
    public long Addition2 { get; set; }
    public long SumAmountSystem { get; set; }
    public long SumAmountWithFish { get; set; }
    public Transaction Transaction { get; set; }
    public string BankAccountNumber { get; set; }
    public bool PayByBankReceipt { get; set; }    



    public void UpdateAmount(long amountSystem, long deduction2)
    {
        AmountSystem = amountSystem;
        Deduction2 = deduction2;
        SumAmountSystem = amountSystem - deduction2;
        SumAmountWithFish = amountSystem - deduction2;
    }

    public void UpdateTransaction(Transaction transaction)
    {
        Transaction = transaction;
    }
}