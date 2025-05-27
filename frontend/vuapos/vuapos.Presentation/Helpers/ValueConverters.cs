using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace vuapos.Presentation.Helpers
{
    public class StockStatus
    {
        public string Text { get; set; }   // Chuỗi hiển thị
        public string Color { get; set; }  // Màu chữ (hex hoặc tên)
        public string Icon { get; set; }   // Ký hiệu FontIcon (Segoe MDL2 Assets)
    }


        public partial class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal amount)
            {
                var culture = new CultureInfo("vi-VN");
                return string.Format(culture, "{0:C0}", amount); // Ví dụ: 123456 -> 123.456 ₫
            }

            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return $"Cập nhật: {dateTime:dd/MM/yyyy HH:mm}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class DateTimeConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return $"{dateTime:dd/MM/yyyy HH:mm}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class EmptyVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int count)
            {
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal d)
                return d.ToString();
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (decimal.TryParse(value as string, out var result))
                return result;
            return 0m; // hoặc DependencyProperty.UnsetValue nếu muốn bỏ qua
        }
    }


    public partial class StockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int stock)
            {
                string mode = parameter as string;

                if (stock > 10)
                {
                    return mode switch
                    {
                        "Text" => $"In Stock ({stock})",
                        "Color" => new SolidColorBrush(Colors.Green),
                        "Icon" => "\uE73E", // CheckMark Icon (Segoe MDL2 Assets)
                        _ => null
                    };
                }
                else if (stock > 0)
                {
                    return mode switch
                    {
                        "Text" => $"Low Stock ({stock})",
                        "Color" => new SolidColorBrush(Colors.Orange),
                        "Icon" => "\uE783", // Warning Icon
                        _ => null
                    };
                }
                else
                {
                    return mode switch
                    {
                        "Text" => "Out of Stock",
                        "Color" => new SolidColorBrush(Colors.Red),
                        "Icon" => "\uEA39", // Blocked Icon
                        _ => null
                    };
                }
            }
            return "N/A";
        }
            public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }

    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !string.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


}
