using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMM
{
    class TextLengthToBackgroundColorConverter : MarkupExtension, IValueConverter
    {
        public static TextLengthToBackgroundColorConverter converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (string)value;

            if (text.Length == 0)
                return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            else
                return new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new TextLengthToBackgroundColorConverter());
        }
    }
}
