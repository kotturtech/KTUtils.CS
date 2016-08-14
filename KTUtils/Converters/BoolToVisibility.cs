using System;
using System.Windows;
using System.Windows.Data;

namespace KotturTech.WPFGoodies.Converters
{
    /// <summary>
    /// Boolean To Visibility converter - Converts bool value to visibility.
    /// Parameter of boolean type indicates whether the visibility value should reserve space in the layout in case
    /// when source value is false (Visibility.Hidden). 
    /// If parameter value is false or null, the result will be Visibility.Collapsed.
    /// </summary>
    [ValueConversion(typeof(bool),typeof(Visibility),ParameterType = typeof(bool?))]
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var param = (parameter as bool?);
            return (bool)value ? Visibility.Visible : (param.HasValue && param.Value ? Visibility.Hidden : Visibility.Collapsed);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible ? true : false;
        }
    }
}
