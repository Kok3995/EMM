using Data;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMM
{
    public class AEActionToStringConverter : MarkupExtension, IValueConverter
    {
        public static AEActionToStringConverter converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AEAction ae)
            {
                switch (ae)
                {
                    case AEAction.EXPBattle:
                        return "Battle Full";
                    case AEAction.TrashMobBattle:
                        return "Battle Partial";
                    case AEAction.BossBattle:
                        return "Boss Battle";
                    case AEAction.FoodAD:
                        return "Food in AD";
                    case AEAction.ReFoodAD:
                        return "Food after AD";
                    case AEAction.Wait:
                        return "Wait";
                    default:
                        return string.Empty;
                }
            }
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new AEActionToStringConverter());
        }
    }
}
