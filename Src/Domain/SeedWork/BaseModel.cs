using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Coreapi.Domain.SeedWork
{
    public class BaseModel<T> 
    {
        [Display(Order = 0, Name = "Identity", AutoGenerateFilter = false)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

    }
}
