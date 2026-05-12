using System;
using AutoMapper;
using Coreapi.Application.Common.Mappings;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.PanelMakerAgg;

namespace Coreapi.Application.Features.PanelMakers.Queries.GetClientPanelMakers
{
    public class PanelMakerDto : IMapFrom<PanelMaker>
    {
        public Guid Id { get; set; }
        public Client Client { get; init; }
        public long NaCode { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public bool IsActive { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string LicenseNumber { get; set; }
        public string SignatureFileName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public int IdSection { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PanelMaker, PanelMakerDto>();
        }
    }
}
