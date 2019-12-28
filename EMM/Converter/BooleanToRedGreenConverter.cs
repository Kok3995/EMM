using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMM
{
    public class BooleanToRedGreenConverter : MarkupExtension, IValueConverter
    {
        public static BooleanToRedGreenConverter converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Application.Current.TryFindResource("GreenBrush");
            else
                return Application.Current.TryFindResource("RedBrush");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new BooleanToRedGreenConverter());
        }
    }
}
