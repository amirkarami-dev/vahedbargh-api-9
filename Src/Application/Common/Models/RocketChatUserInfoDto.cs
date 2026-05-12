using Newtonsoft.Json;
using System.Collections.Generic;
using Coreapi.Application.Common.Models.RocketChat;

namespace Coreapi.Application.Common.Models
{

    public class RocketChatUserInfoDto
    {
        public RocketChatUser[] users { get; set; }
        public bool success { get; set; }
    }

    public class RocketChatUser
    {
        public string _id { get; set; }
        public RocketChatEmail[] emails { get; set; }
        public string name { get; set; }
        public string username { get; set; }
    }

    public class RocketChatEmail
    {
        public string address { get; set; }
    }

}
