using EMM.Core;
using EMM.Core.Tools;
using System.ComponentModel;
using System.Windows;

namespace EMM
{
    /// <summary>
    /// Interaction logic for TimerView.xaml
    /// </summary>
    public partial class TimerView : Window
    {
        public TimerView()
        {
            InitializeComponent();

            Messenger.Register((sender, e) =>
            {
                if (e.TimerMessage != TimerMessage.CloseTimer)
                    return;

                this.Close();
            });
        }
    }
}
