using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Common.Models
{
    public class LoginOutput
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UrlRedirect { get; set; }
    }
}
