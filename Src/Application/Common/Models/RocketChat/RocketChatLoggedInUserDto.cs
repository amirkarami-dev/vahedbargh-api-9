using Newtonsoft.Json;
using System.Collections.Generic;

namespace Coreapi.Application.Common.Models.RocketChat
{
    public class RocketChatLoggedInUserDto
    {
        public string status { get; set; }
        public Data data { get; set; }
    }
    
    public class Data
    {
        public string authToken { get; set; }
        public string userId { get; set; }
    }

}
