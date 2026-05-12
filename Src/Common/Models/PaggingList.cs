using System.Collections.Generic;

namespace Coreapi.Common.Models
{
    public class PaggingList<T>
    {
        public List<T> Data { get; set; }
        public int TotalItems { get; set; }
    }
}
