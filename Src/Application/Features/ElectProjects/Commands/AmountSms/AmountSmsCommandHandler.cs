using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.ElectProjects.Commands.AmountSms;

public class AmountSmsCommandHandler(
    IElectProjectRepository electProjectRepository, 
    ISmsService smsService,
    ITransactionRepository transactionRepository
    )
    : IRequestHandler<AmountSmsCommand, string>
{
    public async Task<string> Handle(AmountSmsCommand request, CancellationToken cancellationToken)
    {
        var electProject = await electProjectRepository.GetElectProjectById(request.ElectProjectId);
        if (electProject is null) throw new NotFoundException("پرونده یافت نشد");

        var encodeGuid = Helper.EncodeGuid(electProject.Id);

		var amount = electProject.IsBigProject ? await transactionRepository.GetBigProjectBalance([.. electProject.ChildProjects.Select(s => s.Id.ToString())]) : await transactionRepository.GetProjectBalance(electProject.Id);


        var amountForPay = Math.Abs(amount);

		switch (request.SmsTypeEnum)
        {
            case SmsTypeEnum.Payment when amountForPay <= 0:
                throw new NotFoundException("مبلغ صحیح نیست");
            // ارسال پیام واریزی به مالک
            case SmsTypeEnum.Payment:
            {
					if (amount >= 0) throw new NotFoundException("بالانس پرونده صفر یا بیشتر می باشد");

					var param = $"e={encodeGuid}&a={amountForPay}";
                await smsService.SendSms4Params(electProject.LandlordPhoneNumber, 9593, electProject.FileNumber.ToString(), Helper.MiladiToShamsi(DateTime.Now),
					//await smsService.SendSms4Params(electProject.LandlordPhoneNumber, 16131, electProject.FileNumber.ToString(), Helper.MiladiToShamsi(DateTime.Now),
					amountForPay.ToString("N0") + "ریال", param);
                break;
            }
            case SmsTypeEnum.Upload:
            {
                // ارسال بارگذاری مدارک توسط مالک
                var param2 = $"e={encodeGuid}";
                await smsService.SendSms3Params(
                    electProject.LandlordPhoneNumber,
                    14646,
                    electProject.FileNumber.ToString(),
                    electProject.ElectRequestNumber.ToString(),
                    param2);
                break;
            }
            default:
                throw new NotFoundException("مشکل در پارامترهای ارسالی");

        }

        return "ok";
    }
}