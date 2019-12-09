using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMM
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public static BooleanToVisibilityConverter _booleanConverter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter?.Equals("reverse") == true)
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _booleanConverter ?? (_booleanConverter = new BooleanToVisibilityConverter());
        }
    }
}
