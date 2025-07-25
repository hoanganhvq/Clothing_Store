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
        [JsonPropertyName("productCode")]
        public string Product_Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Product_Name { get; set; } = string.Empty;
        [JsonPropertyName("stockQuantity")]
        public int Stock_Quantity { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("discount")]
        public int Discount { get; set; }
        [JsonPropertyName("costPrice")]
        public decimal Cost_Price { get; set; }
        [JsonPropertyName("imageUrl")]
        public string Image_Path { get; set; } = string.Empty;
        [JsonPropertyName("categoryName")]
        public string Category_Name { get; set; } = string.Empty;

        [JsonPropertyName("categoryId")]
        public int Category_Id { get; set; }
    }

}
