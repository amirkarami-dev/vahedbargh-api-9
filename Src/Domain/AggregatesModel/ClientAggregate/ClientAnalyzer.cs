using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Domain.AggregatesModel.ClientAggregate
{
    public class ClientAnalyzer
    {
        public ClientAnalyzer(string userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }

        private ClientAnalyzer()
        {
        }

        public Guid Id { get; init; }
        public string UserId { get; init; }
    }
}
