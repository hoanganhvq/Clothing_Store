using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace vuapos.Presentation.Helpers
{
    public class CurrencyFormat : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal priceDecimal)
            {
                CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
                return priceDecimal.ToString("#,### đ", culture.NumberFormat);
            }

            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
