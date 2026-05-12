using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Domain.AggregatesModel.ClientAggregate
{
    public class ClientCard
    {
        public ClientCard(string nameOnCard, string cardNumber, CardTypeEnum type, string cCV, string expireDate)
        {
            Id = Guid.NewGuid();
            CardNumber = cardNumber;
            Type = type;
            CCV = cCV;
            ExpireDate = expireDate;
            NameOnCard = nameOnCard;
        }

        public void Update(string nameOnCard, string cardNumber, string cCV, string expireDate)
        {
            CardNumber = cardNumber;
            CCV = cCV;
            ExpireDate = expireDate;
            NameOnCard = nameOnCard;
        }

        public Guid Id { get; init; }
        public string NameOnCard { get; private set; }
        public string CardNumber { get; private set; }
        public CardTypeEnum Type { get; init; }
        public string CCV { get; private set; }
        public string ExpireDate { get; private set; }
    }
}
