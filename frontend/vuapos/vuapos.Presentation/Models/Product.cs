using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using vuapos.Presentation.Helpers;

namespace vuapos.Presentation.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Product_Id { get; set; }
        [JsonPropertyName("product_code")]
        public string Product_Code { get; set; } = string.Empty;
        //[JsonPropertyName("id")]
        public string Product_Name { get; set; } = string.Empty;
        //[JsonPropertyName("id")]
        public int Stock_Quantity { get; set; }
        //[JsonPropertyName("id")]
        public int Category_Id { get; set; } 

        public decimal Price { get; set; }
        //[JsonPropertyName("id")]
        public int Discount { get; set; }
        //[JsonPropertyName("id")]
        public decimal Cost_Price { get; set; }
        //[JsonPropertyName("id")]
        public string Image_Path { get; set; } = string.Empty;
        //[JsonPropertyName("id")]
        public string Category_Name { get; set; } = string.Empty;
    }

}
