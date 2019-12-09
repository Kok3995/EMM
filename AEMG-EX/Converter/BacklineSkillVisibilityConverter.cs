using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AEMG_EX
{
    public class BacklineSkillVisibilityConverter : MarkupExtension, IMultiValueConverter
    {
        public static BacklineSkillVisibilityConverter converter = null;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var IsSelectedSkillVisible = (bool)values[0];
            var IsBacklineSkillVisible = (bool)values[1];

            if (IsBacklineSkillVisible && IsSelectedSkillVisible)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new BacklineSkillVisibilityConverter());
        }
    }
}
