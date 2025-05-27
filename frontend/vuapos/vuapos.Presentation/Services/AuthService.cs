using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Login;
using vuapos.Presentation.Services.Interfaces;

namespace vuapos.Presentation.Services
{
    public class AuthService : ApiService, IAuthService
    {
     
        private readonly IUserSession _userSession;
        public AuthService(HttpClient httpClient, IUserSession userSession) : base(httpClient)
        {
            _userSession = userSession;
            base.Token = "";
        }

        public async Task<LoginResult> LoginAsync(LoginRequest loginRequest)
        {
            Debug.WriteLine($"LoginRequest: {loginRequest}");
            var res = await SendRequestAsync<LoginResult>(HttpMethod.Post, "auth/login", loginRequest);
            if (res != null)
            {
                _userSession.role = res.Role;
                _userSession.Token = res.Access_token;
                _userSession.UserId = res.Staff_id;
                _userSession.Username = loginRequest.username;
                return res;
            }
            return new LoginResult();
        }

        public void Logout()
        {
            _userSession.Clear();
        }
    }
}
