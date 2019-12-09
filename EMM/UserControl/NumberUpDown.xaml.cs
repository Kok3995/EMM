using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace EMM
{
    /// <summary>
    /// Interaction logic for NumberUpDown.xaml
    /// </summary>
    public partial class NumberUpDown : UserControl
    {
        public NumberUpDown()
        {
            InitializeComponent();
        }

        public int NumbericTextBox
        {
            get { return (int)GetValue(NumbericTextBoxProperty); }
            set { SetValue(NumbericTextBoxProperty, value); }
        }

        public static readonly DependencyProperty NumbericTextBoxProperty =
            DependencyProperty.Register("NumbericTextBox", typeof(int), typeof(NumberUpDown), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsArrange));

        private void UpRepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(numbericTextBox.Text, out int currentNumber))
                NumbericTextBox = (currentNumber + 1 <= 99999) ? (currentNumber += 1) : 99999;
            else
                NumbericTextBox = 1;
        }

        private void DownRepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(numbericTextBox.Text, out int currentNumber))
                NumbericTextBox = (currentNumber - 1 > 0) ? (currentNumber -= 1) : 1;
            else
                NumbericTextBox = 1;
        }
    }
}
