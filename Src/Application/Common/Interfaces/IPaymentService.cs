using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Models;

namespace Coreapi.Application.Common.Interfaces
{
    public interface IPaymentService
    {
        Task<SecurePaymentResponseModel> Pay(string token, string orderId, decimal amount, string currency, string ip);
        Task<StripePaymentResponseModel> Pay(string paymentMethodId, string orderId, decimal amount, string currency);
    }
}
