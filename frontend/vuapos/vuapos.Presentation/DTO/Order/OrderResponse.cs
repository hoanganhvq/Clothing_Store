using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Order
{
    public class OrderResponse
    {
        public string Order_id { get; set; } = string.Empty;
        public string Order_item_id { get; set; } = string.Empty;
        public string Order_date { get; set; } = string.Empty;
        public string Customer_id { get; set; } = string.Empty;
        public string Total_amount { get; set; } = string.Empty;
        public string Staff_id { get; set; } = string.Empty;
    }
}
