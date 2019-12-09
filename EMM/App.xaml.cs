using Data;
using EMM.Core;
using EMM.Core.Converter;
using EMM.Core.Service;
using EMM.Core.ViewModels;
using EMM.Core.Update;
using System.Windows;
using EMM.Core.Tools;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace EMM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //reset working directory
            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var autoMapper = new SimpleAutoMapper();
            var dataIO = new DataIO();
            var messageBox = new MessageBoxService();
            var mouseHook = new MouseHook();
            var timerTool = new TimerTool();
            var autoUpdater = new AutoUpdater(messageBox);

            var DependencyDict = new Dictionary<Type, object>()
            {
                { typeof(SimpleAutoMapper), autoMapper },
                { typeof(MessageBoxService), messageBox }
            };

            var scriptApplyDict = new Dictionary<Emulator, IApplyScriptToFolder>()
            {
                { Emulator.Nox, new NoxScriptApply(messageBox) },
                { Emulator.Memu, new MemuScriptApply(messageBox) }
            };

            var viewModelFactory = new ViewModelFactory(DependencyDict);

            var timerToolVM = new TimerToolViewModel(mouseHook, timerTool);
            var settingVM = new SettingViewModel(Settings.Default(), autoMapper);
            var macroManagerVM = new MacroManagerViewModel(dataIO, messageBox, viewModelFactory);
            var scriptApplyFactory = new ScriptApplyFactory(messageBox, scriptApplyDict);
            var scriptGenerator = new ScriptGenerator(scriptApplyFactory, settingVM, messageBox);
            var scriptGeneratorVM = new ScriptGeneratorViewModel(macroManagerVM, scriptGenerator, messageBox);
            var customActionManager = new CustomActionManager(macroManagerVM, viewModelFactory, dataIO, messageBox);

            var mainWindowViewModel = new MainWindowViewModel(macroManagerVM, settingVM, autoUpdater, timerToolVM, scriptGeneratorVM, customActionManager);

            MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };

            //Handle arguments
            var agrs = Environment.GetCommandLineArgs();
            if (agrs.Length > 1)
            {
                var filepath = agrs[1];

                macroManagerVM.SetCurrentMacro(filepath);
            }

            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Messenger.Send(this, new AppEventArgs(EventMessage.SaveSettings));
            base.OnExit(e);
        }
    }
}
