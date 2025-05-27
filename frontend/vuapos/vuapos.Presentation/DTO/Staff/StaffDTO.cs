using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Staff
{
    public class StaffDTO
    {
        public required String username { get; set; }
        public required String password { get; set; }
        public required String phone { get; set; }
        public required String role { get; set; }
    }
}
