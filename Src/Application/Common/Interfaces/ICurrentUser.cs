using System;
using System.Collections.Generic;
using System.Text;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        string UserId { get; }
        string Ip { get; }
        string Role { get; }
        string ClientId { get; }
    }
}
