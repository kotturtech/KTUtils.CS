using System;
using System.Windows;
using System.Windows.Data;

namespace KotturTech.WPFGoodies.Converters
{
    /// <summary>
    /// Bool negation converter - Converts bool value to negation of the source value
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolNegation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
