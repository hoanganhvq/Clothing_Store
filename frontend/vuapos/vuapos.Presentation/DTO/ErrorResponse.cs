using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO
{

        public class ErrorResponse
        {
            public string error { get; set; }
            public string message { get; set; }
            public string timestamp { get; set; }
            public int status { get; set; }
        }

    
}
