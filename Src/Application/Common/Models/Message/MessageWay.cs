using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Models.Message
{


        public class ResponseMessageWay
        {
            public string Status { get; set; }
            public Error Error { get; set; }

            public long ReferenceID { get; set; }


        }

        public class ResponseSubmitMessageWay
        {
            public string Status { get; set; }
            public Error Error { get; set; }
            
        }

    public class Error
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }
}

