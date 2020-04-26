using Data;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMM
{
    public class BasicActionToImageConverter : MarkupExtension, IValueConverter
    {
        public static BasicActionToImageConverter converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            switch ((BasicAction)value)
            {
                case BasicAction.Click:
                    return Application.Current.TryFindResource("ClickButton");
                case BasicAction.Swipe:
                    return Application.Current.TryFindResource("SwipeButton");
                case BasicAction.Wait:
                    return Application.Current.TryFindResource("WaitButton");
                case BasicAction.AE:
                    return Application.Current.TryFindResource("AEButton");
                case BasicAction.CustomAction:
                    return Application.Current.TryFindResource("CustomButton");
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new BasicActionToImageConverter());
        }
    }
}
