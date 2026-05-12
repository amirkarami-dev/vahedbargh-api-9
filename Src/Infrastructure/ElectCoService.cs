using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models.ElectCo;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using iTextSharp.text.pdf.codec.wmf;
using System.Net;
using RestSharp;

namespace Coreapi.Infrastructure
{
    public class ElectCoService : IElectCoService
    {
        public async Task<string> SendElectProjectFile(ElectCoMapModel electCoMapModel)
        {


            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://nezam.nigc-kd.ir/document");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("x-onme-authorization", "686BA08D6D05009B6CF07DBF75E3BEA8C3524C62");
            var json = JsonConvert.SerializeObject(electCoMapModel).Replace("[", "").Replace("]", "");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var w = client.PostAsync("http://nezam.nigc-kd.ir/document", content);
            w.Wait();
            var response = w.Result;
            return response.StatusCode.ToString();




        }

        public async Task<string> SendElectProjectFileAttach(ElectCoMapAttachModel electCoMapAttachModel)
        {

            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://nezam.nigc-kd.ir/attachment");
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Add("x-onme-authorization", "686BA08D6D05009B6CF07DBF75E3BEA8C3524C62");
            var json = JsonConvert.SerializeObject(electCoMapAttachModel).Replace("[", "").Replace("]", "");
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var w = client.PostAsync("http://nezam.nigc-kd.ir/attachment", content);
            w.Wait();

            var response = w.Result;


            return response.StatusCode.ToString();
        }
    }
}
