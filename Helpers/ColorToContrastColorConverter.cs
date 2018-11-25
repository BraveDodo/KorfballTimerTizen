using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KorfballTimerTizen.Helpers
{
    public class ColorToContrastColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ColorToContrastColor((Color)value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ColorToContrastColor((Color)value);
        }
        public static Color ColorToContrastColor(Color color)
        { // Standard luminance calculation. 
            double luminance = 0.30 * color.R + 0.59 * color.G + 0.11 * color.B;
            return luminance > 0.5 ? Color.Black : Color.White;
        }
    }
}
