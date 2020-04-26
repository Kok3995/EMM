using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AEMG_EX
{
    public class EnumDisplayNameConverter : MarkupExtension, IValueConverter
    {
        public static EnumDisplayNameConverter converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumVal = ((Enum)value);
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            return (attributes.Length > 0 && attributes[0] is DisplayAttribute displayAttribute) ? displayAttribute.Name : null; ;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new EnumDisplayNameConverter());
        }
    } 
}
