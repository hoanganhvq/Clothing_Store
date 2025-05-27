using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace vuapos.Presentation.Models
{
    public class OrderDetail : INotifyPropertyChanged
    {
        public string Order_item_id { get; set; }
        public string Product_id { get; set; }

        private decimal _price;
        private int _quantity;

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    Price = _quantity * Product.Price; // Cập nhật luôn Subtotal
                    OnPropertyChanged();
                }
            }
        }
        public Product Product { get; set; } = new Product();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }
}
