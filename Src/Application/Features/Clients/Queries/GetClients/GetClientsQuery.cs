using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Models;

namespace Coreapi.Application.Features.Clients.Queries.GetClients
{
    public class GetClientsQuery : IRequest<PaggingList<ClientDto>>
    {
        public string Name { get; set; }

        public int PageNumber { get; set; } = 1;
        public int RowCount { get; set; } = 10;
    }
}
