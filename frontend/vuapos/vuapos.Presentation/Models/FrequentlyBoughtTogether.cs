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
        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("product_code")]
        public string ProductCode { get; set; }

        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("category_id")]
        public string CategoryId { get; set; }

        [JsonPropertyName("price")]
        [JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Price { get; set; }

        [JsonPropertyName("cost_price")]
        [JsonConverter(typeof(StringToDecimalConverter))]
        public decimal CostPrice { get; set; }

        [JsonPropertyName("stock_quantity")]
        public int StockQuantity { get; set; }

        [JsonPropertyName("discount")]
        [JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Discount { get; set; }

        [JsonPropertyName("image_path")]
        public string ImagePath { get; set; }

        [JsonPropertyName("category")]
        public Category Category { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
