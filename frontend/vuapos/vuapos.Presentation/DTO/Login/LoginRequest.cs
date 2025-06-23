using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Login
{
    public class LoginRequest
    {
            public required String username { get; set; }
            public required String password { get; set; }
    }
}
