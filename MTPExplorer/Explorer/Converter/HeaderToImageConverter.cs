using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace MTPExplorer
{
    public class HeaderToImageConverter : MarkupExtension, IValueConverter
    {
        public static HeaderToImageConverter converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (DirectoryItemsType)value;

            switch (type)
            {
                case DirectoryItemsType.Drive:
                    return "pack://application:,,,/MTPExplorer;component/Explorer/Images/Mobile.png";

                case DirectoryItemsType.Folder:
                    return "pack://application:,,,/MTPExplorer;component/Explorer/Images/Folder.png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new HeaderToImageConverter());
        }
    }
}
