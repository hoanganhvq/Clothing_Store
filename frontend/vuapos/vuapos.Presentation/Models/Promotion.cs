using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.Models
{
    public class Promotion
    {
        public string Promotion_Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Discount_Percentage { get; set; } = string.Empty;
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
    }
}
