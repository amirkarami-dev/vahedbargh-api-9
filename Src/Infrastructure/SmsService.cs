using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models.Message;
using Coreapi.Common.Models;
using Coreapi.Common.Utility;
using Coreapi.Domain.SeedWork;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Infrastructure
{
    internal class SmsService(IConfiguration configuration, IAuditService _auditService) : ISmsService
    {

        private readonly int _smsProvider = Convert.ToInt32(configuration["SMSService:Provider"]);
        private readonly string _environmentName = configuration["Environment:Name"];
		private readonly long _baleBotId = Convert.ToInt64(configuration["BaleService:BotId"]);
		


		public async Task<ResponseMessageWay> SendSmsCode(string mobileNumber, int templateId,string param1,string param2)
        {

            var data = new
            {
                Method = "sms",
                Provider = _smsProvider,
                Smart = false,
                Mobile = mobileNumber,
                TemplateID = templateId,
                Param1 = param1,
                Param2= param2,
                Length = 4
            };
            var res = await CallSmsApi<ResponseMessageWay>(data, _environmentName);
            return res;

        }

        public async Task<ResponseMessageWay> SendSms2Params(string mobileNumber, int templateId, string param1, string param2)
        {
            var data = new
            {
                Method = "sms",
                Provider = _smsProvider,
                Smart = false,
                Mobile = mobileNumber,
                TemplateID = templateId,
                Param1 = param1,
                Param2 = param2
            };
            var res = await CallSmsApi<ResponseMessageWay>(data, _environmentName);
            return res;
        }

        public async Task<ResponseMessageWay> SendSms3Params(string mobileNumber, int templateId, string param1, string param2, string param3)
        {
            var data = new
            {
                Method = "sms",
                Provider = _smsProvider,
                Smart = false,
                Mobile = mobileNumber,
                TemplateID = templateId,
                Param1 = param1,
                Param2 = param2,
                Param3 = param3
            };
            var res = await CallSmsApi<ResponseMessageWay>(data, _environmentName);
            return res;
        }

        public async Task<ResponseMessageWay> SendSms5Params(string mobileNumber, int templateId, string param1, string param2, string param3,string param4, string param5)
        {
            var data = new
            {
                Method = "sms",
                Provider = _smsProvider,
                Smart = false,
                Mobile = mobileNumber,
                TemplateID = templateId,
                Param1 = param1,
                Param2 = param2,
                Param3 = param3,
                Param4 = param4,
                Param5 = param5
            };
            var res = await CallSmsApi<ResponseMessageWay>(data, _environmentName);
            return res;
        }

        public async Task<ResponseMessageWay> SendSms4Params(string mobileNumber, int templateId, string param1, string param2, string param3, string param4)
        {
            var data = new
            {
                Method = "sms",
                Provider = _smsProvider,
                Smart = false,
                Mobile = mobileNumber,
                TemplateID = templateId,
                Param1 = param1,
                Param2 = param2,
                Param3 = param3,
                Param4 = param4
            };
            var res = await CallSmsApi<ResponseMessageWay>(data, _environmentName);
            return res;
        }
        public async Task<ResponseMessageWay> SendSms1params(string mobileNumber, int templateId, string param1)
        {
            var data = new
            {
                Method = "sms",
                Provider = _smsProvider,
                Smart = false,
                Mobile = mobileNumber,
                TemplateID = templateId,
                Param1 = param1
            };
            var res = await CallSmsApi<ResponseMessageWay>(data, _environmentName);
            return res;
        }

        public async Task<string> SendSmsCodeMfa(string mobileNumber, int templateId, string param1, string code)
        {
            var data = new
            {
                Method = "sms",
                Provider = _smsProvider,
                Smart = false,
                Mobile = mobileNumber,
                TemplateID = templateId,
                Param1 = param1,
                expireTime = 120,
                length = 4
            };
            var res = await CallSmsApi<ResponseMessageWay>(data, _environmentName);
            return res.Status;
        }

        public async Task<ResponseMessageWay> SendSmsCodeParams(string mobileNumber, int templateId, string[] paramArrays, string keyCode)
        {
            var data = new
            {
                Method = "sms",
                Provider = _smsProvider,
                Smart = false,
                Mobile = mobileNumber,
                TemplateID = templateId,
                Param1 = paramArrays[0],
                Param2 = paramArrays[1],
                Param3 = paramArrays[2],
                Length = 4
            };
            if (keyCode != "Abcd@abcd@1234$#@!")
                return new ResponseMessageWay()
                    { Error = new Error() { Code = 0, Message = "error" }, ReferenceID = 0, Status = "error" };
            var res = await CallSmsApi<ResponseMessageWay>(data, _environmentName);
            return res;

        }

        public async Task<ResponseSubmitMessageWay> SmsOtpVerify(string mobileNumber, string otp)
        {
            var data = new
            {
                mobile = mobileNumber,
                OTP = otp
            };
            var res = await VerifySmsApi<ResponseSubmitMessageWay>(data);
            return res;
        }

        public async Task<T> CallSmsApi<T>(object value, string environmentName) where T : new()
        {
            if (environmentName?.Equals("Development", StringComparison.OrdinalIgnoreCase) == true)
                return new T();

            dynamic data = value;
            int templateId = data.TemplateID;

            var parameters = new List<string>();
            for (int i = 1; i <= 5; i++)
            {
                var prop = ((object)data).GetType().GetProperty($"Param{i}");
                var paramValue = prop?.GetValue(data, null)?.ToString();
                if (paramValue != null)
                    parameters.Add(paramValue);
            }

            var message = SmsHelper.BuildMessage(templateId, parameters.ToArray());
            await SmsSave(data.Mobile.ToString(), message == "0" ? templateId.ToString() : message);
			await SendBaleMessageAsync(data.Mobile.ToString(), message == "0" ? templateId.ToString() : message);

			try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("accept-language", "en-IR");
                client.DefaultRequestHeaders.Add("apiKey", "4683e246b9aafb8af069e33a9b0cdddb");

                var json = JsonConvert.SerializeObject(value);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.msgway.com/send", content);
                if (!response.IsSuccessStatusCode)
                    return new T();

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result) ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        public static async Task<T> VerifySmsApi<T>(object value) where T : new()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.msgway.com/otp/verify");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("accept-language", "en-IR");
            client.DefaultRequestHeaders.Add("apiKey", "a5fc4f302ffe69428ee87fc0e704a5f1");
            var json = JsonConvert.SerializeObject(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var w = client.PostAsync("https://api.msgway.com/otp/verify", content);
            w.Wait();

            var response = w.Result;
            if (!response.IsSuccessStatusCode) return await Task.FromResult(new T());
            var result = response.Content.ReadAsStringAsync();
            result.Wait();
            return await Task.FromResult(JsonConvert.DeserializeObject<T>(result.Result));
        }

        public async Task SmsSave(string phoneNumber, string message)
        {
            try
            {
                await _auditService.AddSmsLogsAsync(new ElecSmsLog(phoneNumber, message, "", "", Helper.MiladiToShamsiFull(DateTime.UtcNow.AddHours(3.30)), DateTime.UtcNow, "", ""));
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
            }
        }


		private async Task SendBaleMessageAsync(string mobileNumber, string message)
		{
			using var _baleHttpClient = new HttpClient();
			_baleHttpClient.DefaultRequestHeaders.Add("api-access-key", configuration["BaleService:ApiKey"]);
			try
			{
				var phone = mobileNumber.StartsWith("0")
					? string.Concat("98", mobileNumber.AsSpan(1))
					: mobileNumber;

				var data = new
				{
					bot_id = _baleBotId,
					phone_number = phone,
					message_data = new
					{
						message = new { text = message }
					}
				};

				var json = JsonConvert.SerializeObject(data);
				var content = new StringContent(json);
				content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

				var response = await _baleHttpClient.PostAsync("https://safir.bale.ai/api/v3/send_message", content);
				if (!response.IsSuccessStatusCode)
				{
					var errorBody = await response.Content.ReadAsStringAsync();
					await SmsSave(mobileNumber, $"[Bale Error {(int)response.StatusCode}] {errorBody}");
				}
			}
			catch (Exception ex) when (ex is not OperationCanceledException)
			{
				await SmsSave(mobileNumber, $"[Bale Exception] {ex.Message}");
			}
		}

	}
}
