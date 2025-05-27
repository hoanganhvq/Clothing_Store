using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.Services.Interfaces
{
    public interface IUserSession
    {
        string Username { get; set; }
        string Token { get; set; }
        string role { get; set; }   
        string UserId { get; set; }

        void Clear();
    }

}
