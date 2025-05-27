using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using Windows.UI;
using System.Diagnostics;

namespace vuapos.Presentation.Helpers
{
    // Converter để quyết định hiển thị nút số trang hay không
    public class PageButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intValue)
            {
                // Nếu là số dương thì hiển thị nút, ngược lại ẩn
                return intValue > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    // Converter để thiết lập màu nền cho nút trang
    public class PageBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            if (value is int pageNumber && parameter is int currentPage)
            {
             
                // Nếu là trang hiện tại, đổi màu nền
                return pageNumber == currentPage ?
                    new SolidColorBrush(Color.FromArgb(255, 193, 146, 86)) : // #C19256
                    new SolidColorBrush(Colors.Transparent);
           
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    // Converter để thiết lập màu chữ cho nút trang
    public class PageForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int pageNumber && parameter is int currentPage)
            {
                // Nếu là trang hiện tại, đổi màu chữ thành trắng
                return pageNumber == currentPage ?
                    new SolidColorBrush(Colors.White) :
                    new SolidColorBrush(Colors.Black);
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
