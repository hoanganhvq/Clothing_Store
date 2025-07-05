using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace vuapos.Presentation.Models
{
    public class Staff 
    {
        [JsonPropertyName("id")]
        public int Staff_Id { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}