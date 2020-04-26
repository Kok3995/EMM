using AEMG_EX.Core;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AEMG_EX
{
    public class PositionToImageConverter : MarkupExtension, IValueConverter
    {
        public static PositionToImageConverter converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            switch ((Position)value)
            {
                case Position.First:
                    return AEMGStatic.C1;
                case Position.Second:
                    return AEMGStatic.C2;
                case Position.Third:
                    return AEMGStatic.C3;
                case Position.Fourth:
                    return AEMGStatic.C4;
                case Position.Fifth:
                    return AEMGStatic.C5;
                case Position.Sixth:
                    return AEMGStatic.C6;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new PositionToImageConverter());
        }
    }
}
