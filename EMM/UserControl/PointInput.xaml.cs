using System.Windows;
using System.Windows.Controls;

namespace EMM
{
    /// <summary>
    /// Interaction logic for PointInput.xaml
    /// </summary>
    public partial class PointInput : UserControl
    {
        public PointInput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// X cordinate of the point
        /// </summary>
        public int X
        {
            get { return (int)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(int), typeof(PointInput), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static void OnXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PointInput).X = (int)e.NewValue;
        }

        /// <summary>
        /// Y cordinate of the point
        /// </summary>
        public int Y
        {
            get { return (int)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(int), typeof(PointInput), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
