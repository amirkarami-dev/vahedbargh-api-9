using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Models.Message;
using MediatR;

namespace Coreapi.Application.Common.Interfaces
{
    public interface ISmsService
    {
        Task<ResponseMessageWay> SendSmsCode(string mobileNumber,int templateId, string param1, string param2);
        Task<ResponseMessageWay> SendSms3Params(string mobileNumber,int templateId, string param1, string param2, string param3);
        Task<ResponseMessageWay> SendSms5Params(string mobileNumber, int templateId, string param1, string param2, string param3,string param4, string param5);
        Task<ResponseMessageWay> SendSms4Params(string mobileNumber, int templateId, string param1, string param2, string param3, string param4);

        Task<ResponseMessageWay> SendSms2Params(string mobileNumber, int templateId, string param1, string param2);
        Task<ResponseMessageWay> SendSms1params(string mobileNumber,int templateId, string param1);
        Task<string> SendSmsCodeMfa(string mobileNumber,int templateId, string param1, string code);
        Task<ResponseMessageWay> SendSmsCodeParams(string mobileNumber,int templateId, string[] paramArrays, string keyCode);
        Task<ResponseSubmitMessageWay> SmsOtpVerify(string mobileNumber, string otp);

    }
}
