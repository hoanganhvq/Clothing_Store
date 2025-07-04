using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace vuapos.Presentation.Models
{
    public class Category
    {
        [JsonPropertyName("id")]
        public int Category_Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
