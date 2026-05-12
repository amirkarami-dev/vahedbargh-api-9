using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.RequestModel
{
    public class ElectProjectsFilterModel:BaseRequestModel
    {
        public ElectProjectsFilterModel(string searchValue, long fileNumber, string solarRegisterDate, int page , int pageSize)
        {
            SearchValue = searchValue;
            FileNumber = fileNumber;
            SolarRegisterDate = solarRegisterDate;
            Page = page;
            PageSize = pageSize;
        }
        public string SearchValue { get; set; }
        public long FileNumber { get; set; }
        public string SolarRegisterDate { get; set; }
    }
}
