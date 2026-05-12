using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Utility
{
    public class EnumHelper
    {
        public EnumHelper(Enum @enum)
        {
            Enum = @enum;

        }

        public string GetDisplayName()
        {
            return Enum.GetType()?
                .GetMember(Enum.ToString())?
                .First()?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name;
        }
        public Enum Enum { get; set; }
     
    }
}
