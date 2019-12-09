using System.Windows;
using System.Windows.Controls;

namespace EMM
{
    public class TextBoxAttachProperties
    {

        public static bool GetSelectAllWhenDoubleClicked(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectAllWhenDoubleClickedProperty);
        }

        public static void SetSelectAllWhenDoubleClicked(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectAllWhenDoubleClickedProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectAllWhenDoubleClicked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectAllWhenDoubleClickedProperty =
            DependencyProperty.RegisterAttached("SelectAllWhenDoubleClicked", typeof(bool), typeof(TextBox), new PropertyMetadata(default(bool), new PropertyChangedCallback(OnDoubleClicked)));



        private static void OnDoubleClicked(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (d as TextBox);

            if (textBox == null)
                return;

            if (e.NewValue == e.OldValue)
                return;

            if ((bool)e.NewValue)
            {
                textBox.SelectAll();
            }
        }
    }
}
