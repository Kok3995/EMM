using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AEMG_EX
{
    public class LeftRightToImageConverter : MarkupExtension, IValueConverter
    {
        public static LeftRightToImageConverter converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var option = (Core.Action)value;

            if (option == Core.Action.RunLeft)
                return Application.Current.Resources["CatLeft"];
            if (option == Core.Action.RunRight)
                return Application.Current.Resources["CatRight"];

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new LeftRightToImageConverter());
        }
    }
}
