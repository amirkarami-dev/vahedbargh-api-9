using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Mappings;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetClients
{
    public class ClientDto : IMapFrom<Client>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subdomain { get; set; }
        public string ABN { get; set; }
        public CompanyTypeEnum CompanyType { get; set; }
        public StaffRangeEnum StaffRange { get; set; }
        public decimal Balance { get; set; }
        public int UsersCount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Client, ClientDto>();
        }
    }
}
