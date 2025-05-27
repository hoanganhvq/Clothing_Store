using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace vuapos.Presentation.Helpers
{
    public class UsernameToInitialsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string username && !string.IsNullOrEmpty(username))
            {
                // Get first character of username
                string initial = username[0].ToString().ToUpper();

                // If username contains a space, get the first character after the space
                int spaceIndex = username.IndexOf(' ');
                if (spaceIndex >= 0 && spaceIndex < username.Length - 1)
                {
                    initial += username[spaceIndex + 1].ToString().ToUpper();
                }

                return initial;
            }

            return "?";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class UsernameToColorConverter : IValueConverter
    {
        // Predefined colors for avatars
        private static readonly SolidColorBrush[] AvatarColors = new SolidColorBrush[]
        {
            new SolidColorBrush(Color.FromArgb(255, 91, 95, 199)),   // Blue
            new SolidColorBrush(Color.FromArgb(255, 76, 175, 80)),   // Green
            new SolidColorBrush(Color.FromArgb(255, 255, 152, 0)),   // Orange
            new SolidColorBrush(Color.FromArgb(255, 233, 30, 99)),   // Pink
            new SolidColorBrush(Color.FromArgb(255, 156, 39, 176)),  // Purple
            new SolidColorBrush(Color.FromArgb(255, 244, 67, 54)),   // Red
            new SolidColorBrush(Color.FromArgb(255, 0, 188, 212))    // Cyan
        };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string username && !string.IsNullOrEmpty(username))
            {
                // Get a consistent color based on the username
                int hashCode = username.GetHashCode();
                int index = Math.Abs(hashCode) % AvatarColors.Length;

                return AvatarColors[index];
            }

            // Default color if username is empty
            return new SolidColorBrush(Color.FromArgb(255, 158, 158, 158));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}