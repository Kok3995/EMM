using EMM.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace EMM
{
    /// <summary>
    /// Interaction logic for ResolutionConverterView.xaml
    /// </summary>
    public partial class ResolutionConverterView : Window
    {
        public ResolutionConverterView()
        {
            InitializeComponent();

            Messenger.Register((sender, e) =>
            {
                if (e.ToolMessage != ToolMessage.CloseConverter)
                    return;

                this.Close();
            });
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }
    }
}
