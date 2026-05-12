using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum MaritalStatusTypeEnum
    {
        [Display(Name="مجرد")]
        Single=1,
        
        [Display(Name="متاهل")]
        Married=2
    }
}
