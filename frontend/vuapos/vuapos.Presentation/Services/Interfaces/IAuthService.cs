using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Login;

namespace vuapos.Presentation.Services.Interfaces
{
    public class LoginResult
    {
        public string Access_token { get; set; } = string.Empty;
        public string Staff_id { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(LoginRequest loginRequest);
        void Logout();
    }
}
