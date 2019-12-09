using System.Windows;
using System.ComponentModel;
using EMM.Core.ViewModels;
using EMM.Core;

namespace EMM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Messenger.Register((sender, e) =>
            {
                if (e.TimerMessage != TimerMessage.OpenTimer)
                    return;

                var timerView = new TimerView
                {
                    Topmost = true
                };

                timerView.DataContext = (this.DataContext as MainWindowViewModel).TimerTool;

                timerView.Show();
            });
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var result = MessageBoxResult.No;
            if (vm.MacroManager.CurrentMacro != null && vm.MacroManager.CurrentMacro.IsChanged == true)
            {
                result = MessageBox.Show("The Macro has not been saved. Do you want to save before exiting?", "Exiting", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            }

            switch (result)
            {
                case MessageBoxResult.Cancel:
                case MessageBoxResult.None:
                    e.Cancel = true;
                    break;
                case MessageBoxResult.Yes:
                    Messenger.Send(this, new AppEventArgs(EventMessage.SaveMacroBeforeExit));
                    break;
                case MessageBoxResult.No:
                    break;
            }
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
