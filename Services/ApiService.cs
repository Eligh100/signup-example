using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace signup_example.Services
{
    public class ApiService: IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
          _httpClient = new HttpClient();
        }

        [HttpPost]
        public Task<HttpResponseMessage> Post(string path, object payload)
        {
            var content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
            return _httpClient.PostAsync(path, content);
        }
    }
}
