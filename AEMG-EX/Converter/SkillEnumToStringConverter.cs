using AEMG_EX.Core;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AEMG_EX
{
    public class SkillEnumToStringConverter : MarkupExtension, IValueConverter
    {
        public static SkillEnumToStringConverter converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((CharacterAction)value)
            {
                case CharacterAction.DefaultSkill:
                    return "Default";
                case CharacterAction.SkillOne:
                    return "One";
                case CharacterAction.SkillTwo:
                    return "Two";
                case CharacterAction.SkillThree:
                    return "Three";
                case CharacterAction.Move:
                    return "Move";
                case CharacterAction.Reserve:
                    return "Reserves";
                case CharacterAction.AF0:
                    return "AF Default";
                case CharacterAction.AF1:
                    return "AF One";
                case CharacterAction.AF2:
                    return "AF Two";
                case CharacterAction.AF3:
                    return "AF Three";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new SkillEnumToStringConverter());
        }
    }
}
