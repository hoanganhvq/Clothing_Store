using System.Text.Json.Serialization;

namespace vuapos.Presentation.Models
{
    public class Customer
    {
        [JsonPropertyName("id")]
        public int Customer_Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Point { get; set; } = 0;
    }
}
