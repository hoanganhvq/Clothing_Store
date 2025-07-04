using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace vuapos.Presentation.Models
{
    public class PromotionResponse
    {
        [JsonPropertyName("id")]
        public int Promotion_id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("value")]
        public decimal Discount_percentage { get; set; }
        [JsonPropertyName("startDate")]
        public string Start_date { get; set; } = string.Empty;
        [JsonPropertyName("endDate")]
        public string End_date { get; set; } = string.Empty;
    }
}
