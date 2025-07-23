using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using vuapos.Presentation.Helpers;

namespace vuapos.Presentation.Views.FrequentlyBoughtTogether
{
    public partial class FrequentlyBoughtTogether
    {
        [JsonPropertyName("total_groups")]
        public int TotalGroups { get; set; }

        [JsonPropertyName("groups")]
        public List<ProductGroup> Groups { get; set; }
    }

    public class ProductGroup
    {
        [JsonPropertyName("group_items")]
        public List<Product> GroupItems { get; set; }

        [JsonPropertyName("order_count")]
        public int OrderCount { get; set; }

        public string GroupName { get; set; } = string.Empty;
    }

    public class Product
    {
        [JsonPropertyName("id")]
        public int ProductId { get; set; }

        [JsonPropertyName("productCode")]
        public string ProductCode { get; set; }

        [JsonPropertyName("name")]
        public string ProductName { get; set; }


        [JsonPropertyName("price")]
        [JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Price { get; set; }

        [JsonPropertyName("costPrice")]
        [JsonConverter(typeof(StringToDecimalConverter))]
        public decimal CostPrice { get; set; }

        [JsonPropertyName("stockQuantity")]
        public int StockQuantity { get; set; }

        [JsonPropertyName("discount")]
        [JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Discount { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImagePath { get; set; }

        [JsonPropertyName("categoryName")]
        public string categoryName { get; set; }

        [JsonPropertyName("categoryId")]
        public int categoryId { get; set; }

    }
}
