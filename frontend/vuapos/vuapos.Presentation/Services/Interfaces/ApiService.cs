using System;

namespace vuapos.Presentation.Services
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using vuapos.Presentation.DTO;
    using vuapos.Presentation.Utils;

    public abstract class ApiService
    {
        protected readonly HttpClient _httpClient;
        private static readonly string _baseUrl = "http://localhost:8080/";

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

            if (data != null)
            {


                string jsonData = JsonSerializer.Serialize(data);
                Debug.WriteLine($"✅ Serialized JSON to send: {jsonData}");
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

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"❌ Request failed: {(int)response.StatusCode} {response.StatusCode}");
                    Debug.WriteLine($"❌ Error body: {error}");
                    return default;
                }

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

        //protected async Task<T?> SendRequestAsync<T>(HttpMethod method, string endpoint, object? data = null)
        //{
        //    var request = new HttpRequestMessage(method, endpoint);

        //    if (data != null)
        //    {
        //        string jsonData = JsonSerializer.Serialize(data);
        //        Debug.WriteLine($"✅ Serialized JSON to send: {jsonData}");

        //        request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
        //    }

        //    try
        //    {
        //        var response = await _httpClient.SendAsync(request);

        //        var options = new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        };
        //        options.Converters.Add(new DecimalJsonConverter());

        //        string responseBody = await response.Content.ReadAsStringAsync();

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            Debug.WriteLine($"❌ Request failed: {(int)response.StatusCode} {response.StatusCode}");
        //            Debug.WriteLine($"❌ Error body: {responseBody}");

        //            // 👇 Thử parse message lỗi từ JSON nếu có
        //            try
        //            {
        //                var errorObj = JsonSerializer.Deserialize<ErrorResponse>(responseBody, options);
        //                string message = errorObj?.message ?? $"Request failed with status {(int)response.StatusCode}";
        //                throw new Exception(message);
        //            }
        //            catch
        //            {
        //                // Nếu không parse được, ném lỗi cơ bản
        //                throw new Exception($"Request failed: {response.StatusCode}");
        //            }
        //        }

        //        Debug.WriteLine($"✅ Response: {responseBody}");

        //        return JsonSerializer.Deserialize<T>(responseBody, options);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"🔥 Exception: {ex.Message}");
        //        throw; // 👈 QUAN TRỌNG: ném lại exception để UI xử lý
        //    }
        //}


    }

}
