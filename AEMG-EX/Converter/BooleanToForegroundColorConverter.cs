using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace AEMG_EX
{
    public class BooleanToForegroundColorConverter : MarkupExtension, IValueConverter
    {
        public static BooleanToForegroundColorConverter converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolean = (bool)value;

            var param = (parameter as string).Split(',');

            if (boolean)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(param[0]));
            else
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(param[1]));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new BooleanToForegroundColorConverter());
        }
    }
}
