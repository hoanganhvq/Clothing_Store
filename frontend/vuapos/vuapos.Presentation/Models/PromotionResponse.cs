using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.Models
{
    public class PromotionResponse
    {
        public string Promotion_id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Discount_percentage { get; set; } = string.Empty;
        public string Start_date { get; set; } = string.Empty;
        public string End_date { get; set; } = string.Empty;
    }
}
