using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Order
{
    public class OrderCreateDTO
    {
        public required int customer_id { get; set; }   
        public required String staff_id { get; set; }
        public required Decimal total_amount { get; set; }

        public int promotion_id { get; set; }

        public bool is_cash { get; set; }

        public bool is_use_customer_point { get; set; }

        public decimal point_discount { get; set; }

    }
}
