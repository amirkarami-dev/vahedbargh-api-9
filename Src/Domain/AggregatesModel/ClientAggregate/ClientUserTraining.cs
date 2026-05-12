using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Domain.AggregatesModel.ClientAggregate
{
    public class ClientUserTraining
    {
        public ClientUserTraining(string userId, string title, string link)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Link = link;
            Title = title;
        }

        private ClientUserTraining()
        {
        }

        public Guid Id { get; init; }
        public string UserId { get; init; }
        public string Link { get; private set; }
        public string Title { get; private set; }

        public void Update(string title, string link)
        {
            Link = link;
            Title = title;
        }
    }
}
