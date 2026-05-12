using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.ViewModels
{
    public class ElectProjectProcessFeeViewModel
    {
        public Guid EngId { get; set; }
        public long Fee { get; set; }
        public int CountCancel { get; set; }
        public int CountExpertCancel { get; set; }
        public int CountErtCancel { get; set; }
        public int  CountErtProcess { get; set; }
        public int  CountErtProcessE6 { get; set; }
        public int  SumAreaTestAndDelivery { get; set; }
    }
}
