using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models.Bank;
using Coreapi.Common.Models.IranKishModels;
using Coreapi.Common.Utility.IranKishUtility;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using Stimulsoft.Blockly.Model;

namespace Coreapi.Infrastructure
{
    public class PaymentMelliService(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        : IPaymentMelliService
    {

        private readonly string _paymentMelliCallBackUrl = configuration["BMIMerchant:PaymentMelliCallBackUrl"];
        private readonly string _paymentMelliCallBackPublicUrl = configuration["BMIMerchant:PaymentMelliCallBackPublicUrl"];
        private readonly string _verifyCallBackUrl = configuration["VerifyCallBackUrl"];
        private readonly string _verifyCallBackPublicUrl = configuration["VerifyCallBackPublicUrl"];
        private readonly string _acceptorId = configuration["BMIMerchant:AcceptorId"];
        private readonly string _terminalId = configuration["BMIMerchant:TerminalId"];
        private readonly string _passPhrase = configuration["BMIMerchant:PassPhrase"];
        private readonly string _rsaPublicKey = configuration["BMIMerchant:RsaPublicKey"]?.ToString();
        private readonly string _refererUrl = configuration["BMIMerchant:RefererUrl"];
        private readonly string _verifyBankUrl = configuration["BMIMerchant:VerifyBankUrl"]; 


        public static async Task<T> CallApi<T>(string apiUrl, string value,string referer) where T : new()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Add("Referer",referer);
            // var json = JsonConvert.SerializeObject(value);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(value, Encoding.UTF8, "application/json");

            var w = client.PostAsync(apiUrl, content);
            w.Wait();

            var response = w.Result;
            if (!response.IsSuccessStatusCode) return await Task.FromResult(new T());
            var result = response.Content.ReadAsStringAsync();
            result.Wait();
            return await Task.FromResult(JsonConvert.DeserializeObject<T>(result.Result));

        }

        public async Task<TokenResult> GetPaymentMelliUrl(long amount,string paymentId)
        {
         


            var ipgData = new IPGDataModel()
            {
                TerminalId = _terminalId,
                AcceptorId = _acceptorId,
                RevertURL = _paymentMelliCallBackUrl,
                Amount = amount,
                PaymentId = paymentId,
                RequestId = "10"+ paymentId,
                CmsPreservationId = "",
                TransactionType = TransactionType.Purchase,
                BillInfo = null,
                PassPhrase = _passPhrase,
                RsaPublicKey = _rsaPublicKey
            };

            var request = CreateJsonRequest.CreateJasonRequest(ipgData);

            var jResult = await CallApi<TokenResult>(
                "https://ikc.shaparak.ir/api/v3/tokenization/make", request, _refererUrl);

            return jResult;
        }

        public async Task<TokenResult> GetPaymentMelliPublicUrl(long amount, string paymentId)
        {
            string webRootPath = hostingEnvironment.ContentRootPath;

            var doc = new XmlDocument();
            doc.Load(Path.Combine(webRootPath, "Upload", "DataXmlFile.xml"));

            var ipgData = new IPGDataModel()
            {
                TerminalId = _terminalId,
                AcceptorId = _acceptorId,
                PaymentId = paymentId,
                RequestId = "10" + paymentId,
                RevertURL = _paymentMelliCallBackPublicUrl,
                Amount = amount,
                MultiplexParameters = null,
                CmsPreservationId = doc.SelectNodes("DocumentElement/IPGData")[0].SelectNodes("CmsPreservationId")[0].InnerText,
                TransactionType = TransactionType.Purchase,
                BillInfo = null,
                PassPhrase = _passPhrase,
                RsaPublicKey = doc.SelectNodes("DocumentElement/IPGData/RSAPublicKey")[0].InnerXml.ToString()
        };

            var request = CreateJsonRequest.CreateJasonRequest(ipgData);



            var jResult = await CallApi<TokenResult>(
                "https://ikc.shaparak.ir/api/v3/tokenization/make", request, _refererUrl);

          

            return jResult;
        }

        public async Task<VerifyResult> VerifyPaymentMelli(string retrievalReferenceNumber, string systemTraceAuditNumber, string token)
        {
            var requestVerify = new RequestVerify();
            requestVerify.TerminalId = _terminalId;
            requestVerify.RetrievalReferenceNumber = retrievalReferenceNumber;
            requestVerify.SystemTraceAuditNumber = systemTraceAuditNumber;
            requestVerify.TokenIdentity = token;

            var request = JsonConvert.SerializeObject(requestVerify);

            var jResult = await CallApi<VerifyResult>(_verifyBankUrl, request, _refererUrl);

            return jResult;
        }

        public string GetVerifyCallBackUrl()
        {
            return _verifyCallBackUrl;
        }

        public string GetVerifyCallBackPublicUrl()
        {
            return _verifyCallBackPublicUrl;
        }

    }

}
