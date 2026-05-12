using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Exceptions
{
    public class NotAllowedException : Exception
    {
        public NotAllowedException()
        {
        }
        public NotAllowedException(string name)
            : base(name)
        {
        }
    }
}
