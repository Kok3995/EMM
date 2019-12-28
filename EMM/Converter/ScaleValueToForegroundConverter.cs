using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMM
{
    public class ScaleValueToForegroundConverter : MarkupExtension, IMultiValueConverter
    {
        public static ScaleValueToForegroundConverter converter = null;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return Application.Current.TryFindResource("RedBrush");

            var scaleX = (double)values[0];
            var scaleY = (double)values[1];

            if (scaleX == scaleY && scaleX > 0 && scaleY > 0)
                return Application.Current.TryFindResource("GreenBrush");
            else return Application.Current.TryFindResource("RedBrush");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new ScaleValueToForegroundConverter());
        }
    }
}
