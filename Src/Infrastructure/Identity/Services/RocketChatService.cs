using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;
using Coreapi.Application.Common.Models.RocketChat;

namespace Coreapi.Infrastructure.Identity
{
    public class RocketChatService : IRocketChatService
    {

        public async Task<bool> CheckToken(string chatUrl, string token, string userId)
        {
            string url = string.Format("{0}{1}", chatUrl, "/api/v1/me");

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("X-Auth-Token", token);
            client.DefaultRequestHeaders.Add("X-User-Id", userId);

            var response = await client.GetAsync(url);

            return response.IsSuccessStatusCode;
        }

        public async Task<RocketChatUserInfoDto> GetAllUsers(string baseUrl, string authToken, string userId)
        {
            string url = string.Format("{0}{1}", baseUrl, "/api/v1/users.list");

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("X-Auth-Token", authToken);
            client.DefaultRequestHeaders.Add("X-User-Id", userId);

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            string result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<RocketChatUserInfoDto>(result);
        }

        public async Task<RocketChatLoggedInUserDto> Login(string baseUrl, string user, string password)
        {
            string url = string.Format("{0}{1}", baseUrl, "/api/v1/login");

            var requestBody = JsonConvert.SerializeObject(new { user, password });

            var client = new HttpClient();

            var data = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);

            if (!response.IsSuccessStatusCode)
                return null;

            string result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<RocketChatLoggedInUserDto>(result);
        }

    }
}
