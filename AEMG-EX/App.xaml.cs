using AEMG_EX.Core;
using Data;
using EMM.Core;
using EMM.Core.Converter;
using EMM.Core.Service;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AEMG_EX
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrap = new Bootstrap();
            MainWindow = bootstrap.SetupMainWindow();

            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Messenger.Send(this, new AppEventArgs(EventMessage.SaveSettings));
            base.OnExit(e);
        }
    }
}
