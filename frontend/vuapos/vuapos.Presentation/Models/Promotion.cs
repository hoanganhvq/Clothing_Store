using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace vuapos.Presentation.Models
{
    public class Promotion
    {
        [JsonPropertyName("id")]
        public int Promotion_Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public decimal Discount_Percentage { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime Start_Date { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime End_Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        public string DiscountDisplay
        {
            get
            {
              
                    return (Discount_Percentage * 100).ToString("0.#") + "%";
              
            }
        }

    }
}
