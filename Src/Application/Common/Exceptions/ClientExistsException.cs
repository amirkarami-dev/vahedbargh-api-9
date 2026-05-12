using System;

namespace Coreapi.Application.Common.Exceptions
{
    public class ClientExistsException : Exception
    {
        public ClientExistsException(string name)
            : base($"Client with email: \"{name}\" is already registerd.")
        {
        }
    }
}
