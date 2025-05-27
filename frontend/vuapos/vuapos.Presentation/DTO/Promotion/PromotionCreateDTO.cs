using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Promotion
{
    public class PromotionCreateDTO
    {
        public string name { get; set; } = string.Empty;
        public decimal discount_percentage { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
}
