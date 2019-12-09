using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AEMG_EX
{
    /// <summary>
    /// Interaction logic for CutCornerBox.xaml
    /// </summary>
    public partial class CutCornerBox : UserControl
    {
        public CutCornerBox()
        {
            InitializeComponent();
        }




        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CutCornerBox), new PropertyMetadata(string.Empty));




    }
}
