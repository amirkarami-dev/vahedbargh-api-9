using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Features.Clients.Commands.UpsertCard
{
    public class UpsertClientCardCommand : IRequest<Guid>
    {
        public Guid? Id { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string ExpireDate { get; set; }
        public string CCV { get; set; }
        public CardTypeEnum Type { get; set; }
    }
}
