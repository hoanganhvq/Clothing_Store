using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Services.Interfaces;

namespace vuapos.Presentation.Services
{
    // Services/UserSession.cs
    public class UserSession : IUserSession
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string role { get; set; }    // Thêm thuộc tính role

        public void Clear()
        {
            Username = null;
            Token = null;
            UserId = "";
            role = null; 
        }
    }

}
