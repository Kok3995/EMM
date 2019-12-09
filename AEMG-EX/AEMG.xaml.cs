using AEMG_EX.Core;
using EMM.Core;
using System;
using System.Windows;

namespace AEMG_EX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AEMG : Window
    {
        public AEMG()
        {
            InitializeComponent();
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Messenger.Send(this, new DropEventArgs((string[])e.Data.GetData(DataFormats.FileDrop)));
            }
        }
    }
}
