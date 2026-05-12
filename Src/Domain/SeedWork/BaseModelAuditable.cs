using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Coreapi.Domain.SeedWork
{
    public class BaseModelAuditable<T> : BaseModel<T>
    {
        public DateTime Created { get; protected set; }
        public DateTime? LastModified { get; protected set; }
        [Display(Name = "System Lock")]
        [DefaultValue(false)]
        public bool Lock { get; set; }
    }

    public class BaseModelWithTotalRow<T>
    {
        public IEnumerable<T> AggregateModel { get; set; }
        public int TotalItem { get; set; }
    }
}
