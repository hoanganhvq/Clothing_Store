using System.Text.Json.Serialization;

namespace vuapos.Presentation.Models
{
    public class Customer
    {
        [JsonPropertyName("id")]
        public int Customer_Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("point")]
        public int Point { get; set; } = 0;
    }
}
