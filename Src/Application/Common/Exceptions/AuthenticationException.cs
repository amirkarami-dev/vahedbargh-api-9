using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Common.Exceptions
{
    public class AuthenticationException : Exception
    {
        public string[] Errors { get; set; }
        public AuthenticationException():base("Register Failed!")
        {

        }
        public AuthenticationException(string[] Errors)
        {
            this.Errors = Errors;
        }
    }
}
