using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.EngPaymentCustom
{
    public class EngPaymentCustomCommand:IRequest<string>
    {
        public Guid EngineerId { get; set; }
        public long Amount { get; set; }
        public string Des { get; set; }
        public string BtId { get; set; }
        public string FishNumber { get; set; }
        public string SolarFishDate { get; set; }
        public TransactionStatusEnum TransactionStatus { get; set; }
    }
}
