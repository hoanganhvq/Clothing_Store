using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Order
{
    public class OrderCreateDTO
    {
        public required String customer_id { get; set; }
        public required String staff_id { get; set; }
        public required Decimal total_amount { get; set; }
    }
}
