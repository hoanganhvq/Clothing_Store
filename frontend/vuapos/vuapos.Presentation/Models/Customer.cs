namespace vuapos.Presentation.Models
{
    public class Customer
    {
        public string Customer_Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Point { get; set; } = 0;
    }
}
