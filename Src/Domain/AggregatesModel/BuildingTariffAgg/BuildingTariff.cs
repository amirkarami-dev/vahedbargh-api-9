using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Domain.AggregatesModel.BuildingTariffAgg
{
    public class BuildingTariff
    {
        private BuildingTariff(){}

        public BuildingTariff( Client client, string userId, long tariff,
            BuildingGroupTypeEnum buildingGroupTypeEnum,
            BuildingGroupParameterTypeEnum buildingGroupParameterTypeEnum, 
            DateTime julianDateExecute, string solarDateExecute, 
            float factor,float testAndDeliveryFactor)
        {
            Id = Guid.NewGuid();
            Client = client;
            UserId = userId;
            Tariff = tariff;
            BuildingGroupTypeEnum = buildingGroupTypeEnum;
            BuildingGroupParameterTypeEnum = buildingGroupParameterTypeEnum;
            JulianDateExecute = julianDateExecute;
            SolarDateExecute = solarDateExecute;
            Factor = factor;
            TestAndDeliveryFactor = testAndDeliveryFactor;
        }
        public Guid Id { get; init; }
        public Client Client { get; init; }
        public string UserId { get; private set; }
        public BuildingGroupTypeEnum BuildingGroupTypeEnum { get; set; }
        public long Tariff { get; private set; }
        public long MinTariff { get; set; }
        public BuildingGroupParameterTypeEnum BuildingGroupParameterTypeEnum { get; private set; }
        public DateTime JulianDateExecute { get; private set; }
        public string SolarDateExecute { get; set; }
        public int SolarYear { get; set; }
        public float Factor { get; set; }
        public float TestAndDeliveryFactor { get; set; }
        public long SupervisionTariff { get; set; }
        public long SupervisionMinTariff { get; set; }
        public float SupervisionFactor { get; set; }
    }
}
