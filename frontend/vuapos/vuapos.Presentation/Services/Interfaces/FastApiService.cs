using System;
using System.Net.Http;

namespace vuapos.Presentation.Services
{
    public class FastApiService : ApiService
    {
        public FastApiService(HttpClient httpClient) : base(httpClient)
        {
            _httpClient.BaseAddress = new Uri("http://localhost:8000/");
        }
    }
}
