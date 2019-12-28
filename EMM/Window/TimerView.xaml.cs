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
                if (e.ToolMessage != ToolMessage.CloseTimer)
                    return;

                this.Close();
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.DataContext is TimerToolViewModel viewModel)
            {
                if (viewModel.IsTimerStart)
                    viewModel.UnHookMouseCommand.Execute(null);
            }

            base.OnClosing(e);
        }
    }
}
