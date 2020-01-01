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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

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
            
            var settingVM = new SettingViewModel(Settings.Default(), autoMapper);
            var macroManagerVM = new MacroManagerViewModel(dataIO, messageBox, viewModelFactory);
            var scriptApplyFactory = new ScriptApplyFactory(messageBox, scriptApplyDict);
            var scriptGenerator = new ScriptGenerator(scriptApplyFactory, settingVM, messageBox);
            var scriptGeneratorVM = new ScriptGeneratorViewModel(macroManagerVM, scriptGenerator, messageBox);
            var customActionManager = new CustomActionManager(macroManagerVM, viewModelFactory, dataIO, messageBox);

            var timerToolVM = new TimerToolViewModel(mouseHook, timerTool);
            var resulutionTool = new ResolutionConverterTool(viewModelFactory);
            var resolutionToolVM = new ResolutionConverterToolViewModel(resulutionTool, macroManagerVM, messageBox);
            var autoLocationVM = new AutoLocationViewModel(new MouseHook(), new AutoLocation(), macroManagerVM);

            var mainWindowViewModel = new MainWindowViewModel(macroManagerVM, settingVM, autoUpdater, timerToolVM, resolutionToolVM, scriptGeneratorVM, customActionManager, autoLocationVM);

            MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };

            //Handle arguments
            var agrs = Environment.GetCommandLineArgs();
            if (agrs.Length > 1)
            {
                var filepath = agrs.Where(s => s.Contains(".emm")).First();

                macroManagerVM.SetCurrentMacro(filepath);
            }

            // Select the text in a TextBox when it receives focus.
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(SelectivelyIgnoreMouseButton));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotKeyboardFocusEvent,
                new RoutedEventHandler(SelectAllText));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.MouseDoubleClickEvent,
                new RoutedEventHandler(SelectAllText));

            MainWindow.Show();
        }

        void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focused, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Messenger.Send(this, new AppEventArgs(EventMessage.SaveSettings));

            base.OnExit(e);
        }
    }
}
