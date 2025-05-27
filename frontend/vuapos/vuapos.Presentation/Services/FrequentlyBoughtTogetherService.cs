using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Views.FrequentlyBoughtTogether;

namespace vuapos.Presentation.Services
{
    public class FrequentlyBoughtTogetherService: ApiService
    {
        public FrequentlyBoughtTogetherService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjhmOWUwNmUxLTM1ZWQtNDViYy05M2Y2LWExN2YyZGIyNmMzOSIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ1NjYxODA5LCJleHAiOjE3NDYyNjY2MDl9.3Myou0ILU61jkT4B0Xv75qrQA7qGWBOegBCREpjnEoI";
        }
        public async Task<FrequentlyBoughtTogether?> GetFrequentlyBoughtTogetherAsync()
        {
            Debug.WriteLine("Sending request to /analyze");
            return await SendRequestAsync<FrequentlyBoughtTogether>(HttpMethod.Get, "analyze");
        }
    }
}
