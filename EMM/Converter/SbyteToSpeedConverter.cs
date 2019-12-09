using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMM
{
    public class SbyteToSpeedConverter : MarkupExtension, IValueConverter
    {
        public static SbyteToSpeedConverter converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((sbyte)value)
            {
                case 5:
                    return "Very Slow";
                case 10:
                    return "Slow";
                case 20:
                    return "Normal";
                case 30:
                    return "Fast";
                case 40:
                    return "Very Fast";
                case 50:
                    return "Flick";
                default:
                    return "Normal";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)value)
            {
                case "Very Slow":
                    return (sbyte)5;
                case "Slow":
                    return (sbyte)10;
                case "Normal":
                    return (sbyte)20;
                case "Fast":
                    return (sbyte)30;
                case "Very Fast":
                    return (sbyte)40;
                case "Flick":
                    return (sbyte)50;
                default:
                    return (sbyte)20;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new SbyteToSpeedConverter());
        }
    }
}
