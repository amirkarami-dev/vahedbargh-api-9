using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.ViewModels
{
    public class ClientEngWorkViewModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public int Year { get; set; }
        public long RemainingQuota { get; set; }
        public long SumAmountEngQuota { get; set; }
        public long CountErtQuota { get; set; }
        public long SumAmountEngQuotaBeforePeriod { get; set; }
        public long CountErtQuotaBeforePeriod { get; set; }
        public long RemainingCurrentQuarterQuota { get; set; }
        public long TotalQuotaBalance { get; set; }
        public long CountErtQuotaBalance { get; set; }
        public long SumAmountEngProcessFee { get; set; }
        public long CountErtProcessFee { get; set; }
        public long SumAmountEngProcessFeeBeforePeriod { get; set; }
        public long CountErtProcessFeeBeforePeriod { get; set; }

        public long SumAmountEngProcessCancelFee { get; set; }
        public long SumEngBalance { get; set; }
        public long SumAmountEngRealWordBefore { get; set; }
        public long SumAmountEngRealWordThisQuarter { get; set; }
        public long SumAmountEngRealWorkQuarterFirstMonth { get; set; }
        public long SumAmountEngRealWorkQuarterSecondMonth { get; set; }
        public long SumAmountEngRealWorkQuarterThirdMonth { get; set; }
        public IEnumerable<EngineerHistoryViewModel> EngineerHistoryViewModel { get; set; }
        public long DefaultQuota { get; set; }

    }

}
