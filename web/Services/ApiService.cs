using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
        public async Task<HttpResponseMessage> Post(string path, object payload)
        {
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"http://localhost:7000/api/{path}", content);
            return response;
        }
    }
}
