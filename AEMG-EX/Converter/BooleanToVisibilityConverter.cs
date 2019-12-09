using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AEMG_EX
{
    public class NullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public static NullToVisibilityConverter converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter?.Equals("reverse") == true)
                return (value != null) ? Visibility.Collapsed : Visibility.Visible;
            return (value != null) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new NullToVisibilityConverter());
        }
    }
}
