using System;
using Coreapi.Common.Enums;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.AmountSms;

public class AmountSmsCommand:IRequest<string>
{
    public Guid ElectProjectId { get; set; }
    public long Amount { get; set; }
    public SmsTypeEnum SmsTypeEnum { get; set; } = SmsTypeEnum.Payment;
}