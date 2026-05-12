using System;

namespace Coreapi.Common.ViewModels;

public class EngPaymentViewModel
{
    public long Amount { get; set; }
    public Guid EngineerId { get; set; }
}