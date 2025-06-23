using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.Models
{
    public class CashRegister
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime LastClosedAt { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
