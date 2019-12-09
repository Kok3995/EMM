using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace AEMG_EX
{
    /// <summary>
    /// TextBox that only accept numberic value
    /// </summary>
    public class NumbericTextBox : TextBox
    {
        private static readonly Regex regex = new Regex("^[0-9]+$");

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

            base.OnKeyDown(e);
        }

        public NumbericTextBox()
        {
            this.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
        }
    }
}
