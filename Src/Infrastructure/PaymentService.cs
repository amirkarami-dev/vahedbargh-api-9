using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;
using IdentityModel.Client;

namespace Coreapi.Infrastructure
{
    public class PaymentService : IPaymentService
    {
        private readonly string _merchantCode;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _secureBaseUrl;
        private readonly string _authenticationUrl;
        private readonly string _stripeApiKey;
        private static string ApiToken { get; set; }
        private static DateTime ExpiredDate { get; set; }

        public PaymentService(IConfiguration configuration)
        {
            _merchantCode = configuration["SecurePayMerchantCode"];
            _clientId = configuration["SecurePayClientId"];
            _clientSecret = configuration["SecurePayClientSecret"];
            _secureBaseUrl = configuration["SecurePayBaseUrl"];
            _authenticationUrl = configuration["SecurePayAuthenticationUrl"];
            _stripeApiKey = configuration["StripeApiKey"];
        }

        private async Task Authenticate()
        {
            ApiToken = string.Empty;
            ExpiredDate = DateTime.Now;

            string url = string.Format("{0}{1}", _authenticationUrl, "/oauth/token");

            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), url);
            var contentList = new List<string>
                    {
                        $"grant_type={Uri.EscapeDataString("client_credentials")}",
                        $"audience={Uri.EscapeDataString("https://api.payments.auspost.com.au")}"
                    };
            request.Content = new StringContent(string.Join("&", contentList));
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
            request.SetBasicAuthentication(_clientId, _clientSecret);
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuthenticateResponse>(content);

                ApiToken = result.access_token;
                ExpiredDate = DateTime.Now.AddSeconds(result.expires_in);
            }

        }

        public async Task<SecurePaymentResponseModel> Pay(string token, string orderId, decimal amount, string currency, string ip)
        {
            if (string.IsNullOrEmpty(ApiToken) || DateTime.Now >= ExpiredDate)
                await Authenticate();

            string url = string.Format("{0}{1}", _secureBaseUrl, "/v2/payments");

            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), url);

            var body = JsonConvert.SerializeObject(new
            {
                merchantCode = _merchantCode,
                amount = amount * 100,
                token,
                ip,
                orderId,
                currency = currency.ToUpper()
            });

            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request.SetBearerToken(ApiToken);
            var response = await httpClient.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<SecurePaymentResponseModel>(content);
            }

            return null;
        }

        public async Task<StripePaymentResponseModel> Pay(string paymentMethodId, string orderId, decimal amount, string currency)
        {
            StripeConfiguration.ApiKey = _stripeApiKey;
            var paymentIntentService = new PaymentIntentService();
            var createOptions = new PaymentIntentCreateOptions
            {
                PaymentMethod = paymentMethodId,
                Amount = (long)(amount * 100),
                Currency = currency,
                ConfirmationMethod = "manual",
                Confirm = false,
                Metadata = new Dictionary<string, string>
                {
                    { "OrderId", orderId }
                }
            };
            var paymentIntent = await paymentIntentService.CreateAsync(createOptions);

            var confirmOptions = new PaymentIntentConfirmOptions { };
            paymentIntent = await paymentIntentService.ConfirmAsync(
                paymentIntent.Id,
                confirmOptions
            );

            return GeneratePaymentResponse(paymentIntent);
        }

        private static StripePaymentResponseModel GeneratePaymentResponse(PaymentIntent intent)
        {
            // Note that if your API version is before 2019-02-11, 'requires_action'
            // appears as 'requires_source_action'.
            if (intent.Status == "requires_action" &&
                intent.NextAction.Type == "use_stripe_sdk")
            {
                // Tell the client to handle the action
                return new StripePaymentResponseModel
                {
                    InvoiceId = intent.Metadata.GetValueOrDefault("OrderId"),
                    Status = false,
                    Amount = intent.Amount / 100,
                    Currency = intent.Currency,
                    Id = intent.Id
                };
            }
            else if (intent.Status == "succeeded")
            {
                // The payment didn’t need any additional actions and completed!
                // Handle post-payment fulfillment
                return new StripePaymentResponseModel
                {
                    Amount = intent.Amount / 100,
                    Currency = intent.Currency,
                    Id = intent.Id,
                    InvoiceId = intent.Metadata.GetValueOrDefault("OrderId"),
                    Status = true
                };
            }
            else
            {
                // Invalid status
                throw new BadRequestException("Invalid PaymentIntent status");
            }
        }
        
        private class AuthenticateResponse
        {
            #pragma warning disable IDE1006 // Naming Styles
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
            #pragma warning restore IDE1006 // Naming Styles
        }

    }
}
