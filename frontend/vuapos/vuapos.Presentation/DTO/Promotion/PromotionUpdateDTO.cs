using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Promotion
{
    public class PromotionUpdateDTO
    {
        public string name { get; set; } = string.Empty;


        private decimal _discountPercentage;
        public decimal discount_percentage
        {
            get => _discountPercentage;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Discount percentage must be between 0 and 100.");
                }
                _discountPercentage = value / 100; // tự động chia
            }
        }
        public string start_date { get; set; }
        public string end_date { get; set; }
       
    }
}
