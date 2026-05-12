using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Domain.AggregatesModel.QuarterTariffAgg
{
    public class QuarterTariff
    {
        private QuarterTariff(){}

        public QuarterTariff(QuarterTypeEnum quarterTypeEnum, int year, AllotmentRoundTypeEnum allotmentRoundTypeEnum, 
            long fee, bool isQuota, DateTime julianAllotmentDate, string solarAllotmentDate,int period,long ertFee, long testAndDeliveryFee, int countErt,int countTestAndDelivery, long ertApprovedFee, int addDifDays)
        {
            Id = Guid.NewGuid();
            QuarterTypeEnum = quarterTypeEnum;
            Year = year;
            AllotmentRoundTypeEnum = allotmentRoundTypeEnum;
            Fee = fee;
            IsQuota = isQuota;
            JulianAllotmentDate = julianAllotmentDate;
            SolarAllotmentDate = solarAllotmentDate;
            Period = period;
            ErtFee = ertFee;
            TestAndDeliveryFee = testAndDeliveryFee;
            CountErt = countErt;
            CountTestAndDelivery = countTestAndDelivery;
            ErtApprovedFee = ertApprovedFee;
            AddDifDays = addDifDays;
        }

        public Guid Id { get; init; }
        public QuarterTypeEnum QuarterTypeEnum { get; private set; }
        public int Year { get; private set; }
        public AllotmentRoundTypeEnum AllotmentRoundTypeEnum { get; private set; }
        public long Fee { get; private set; }
        // میزان تخصیص به مجری های ارت
        public long ErtFee { get; set; }
        public int CountErt { get; set; }
        public long TestAndDeliveryFee { get; set; }
        public int CountTestAndDelivery { get; set; }
        public bool IsQuota { get; private set; } = false;
        public DateTime JulianAllotmentDate { get; private set; }
        public string SolarAllotmentDate { get; private set; }
        public int Period { get; private set; }
        public float PercentIncrease { get; set; }
        public long ErtApprovedFee { get; set; }
        public int AddDifDays { get; set; }

        public void DoQuota()
        {
            IsQuota = true;
        }


    }
}
