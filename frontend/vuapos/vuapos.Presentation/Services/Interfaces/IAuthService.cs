using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Login;

namespace vuapos.Presentation.Services.Interfaces
{
    using System.Text.Json.Serialization;

    public class LoginResult
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("access_token")]
        public string Access_token { get; set; }

        [JsonPropertyName("staff_id")]
        public int Staff_id { get; set; }
    }


    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(LoginRequest loginRequest);
        void Logout();
    }
}
