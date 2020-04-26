using System.Windows;
using System.ComponentModel;
using EMM.Core.ViewModels;
using EMM.Core;
using System.Diagnostics;

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
                if (e.ToolMessage != ToolMessage.OpenTimer)
                    return;

                var timerView = new TimerView
                {
                    Topmost = true
                };

                timerView.DataContext = (this.DataContext as MainWindowViewModel).TimerTool;

                timerView.Show();
            });

            Messenger.Register((sender, e) =>
            {
                if (e.ToolMessage != ToolMessage.OpenConverter)
                    return;

                var resolutionConverterView = new ResolutionConverterView();

                resolutionConverterView.DataContext = (this.DataContext as MainWindowViewModel).ResolutionConverterTool;

                resolutionConverterView.Show();
            });
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            var result = MessageBoxResult.No;
            if (vm.MacroManager.CurrentMacro != null && vm.MacroManager.CurrentMacro.IsDirty())
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
                    //unhook autolocation handler
                    if (vm.AutoLocation.IsCaptureStarted)
                        vm.AutoLocation.ToggleCaptureLocationCommand.Execute(null);
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

        private void About_Click(object sender, RoutedEventArgs e)
        {
            (new AboutView()).ShowDialog();
            e.Handled = true;
        }

        private void Donate_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.paypal.me/kok3995/1"));
            e.Handled = true;
        }

        private void Howto_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.kok-emm.com/How-to"));
            e.Handled = true;
        }

        private void Hotkeys_Click(object sender, RoutedEventArgs e)
        {
            (new HotkeysView()).Show();
            e.Handled = true;
        }
    }
}
