using System;

namespace vuapos.Presentation.Services
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using vuapos.Presentation.Utils;

    public abstract class ApiService
    {
        protected readonly HttpClient _httpClient;
        private static readonly string _baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:3000/";

        private string? _token;
        public string? Token
        {
            set
            {
                _token = value;
                if (!string.IsNullOrEmpty(_token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                }
            }
        }

        protected ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async Task<T?> SendRequestAsync<T>(HttpMethod method, string endpoint, object? data = null)
        {
            var request = new HttpRequestMessage(method, endpoint);
            Console.WriteLine($"Request: {request}");
            if (data != null)
            {
                Debug.WriteLine($"Data: {data}");
                string jsonData = JsonSerializer.Serialize(data);
                request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            }

            try
            {
                var response = await _httpClient.SendAsync(request);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new DecimalJsonConverter());
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response: {responseBody}");
                return JsonSerializer.Deserialize<T>(responseBody, options);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return default;
            }
        }
    }

}
