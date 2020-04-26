using AEMG_EX.Core;
using Data;
using EMM.Core;
using EMM.Core.Converter;
using EMM.Core.Service;
using EMM.Core.Update;
using EMM.Core.ViewModels;
using MTPExplorer;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace AEMG_EX
{
    public class Bootstrap
    {
        public AEMG SetupMainWindow()
        {
            //reset working directory
            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //set culture
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var dataIO = new DataIO();
            var autoMapper = new SimpleAutoMapper();
            var messageBox = new MessageBoxService();
            var repository = new AERepository();
            var mtpManager = new MTPManager();
            var saverepo = new AESavedSetupRepository();

            var actionProvider = new ActionProvider(autoMapper, messageBox);
            var aeActionFactory = new AEActionFactory(actionProvider);

            var DependencyDict = new Dictionary<Type, object>()
            {
                { typeof(SimpleAutoMapper), autoMapper },
                { typeof(MessageBoxService), messageBox },
                { typeof(ViewModelClipboard), new ViewModelClipboard() },
            };

            var viewModelFactory = new ViewModelFactory(DependencyDict);
            var appSettingVM = new ApplicationSettingViewModel(actionProvider, viewModelFactory);
            SetupWindowInit(appSettingVM, out IWindowInitlizer windowinit);

            (new ScriptGenerateBootstrap()).SetUp(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory);

            var scriptApply = new ScriptApplyBootStrap(messageBox, mtpManager).GetScriptApplyFactory();
            var scanner = new MacroScanner(dataIO, aeActionFactory);
            var macroManager = new AEMacroManager(scanner);
            var macroManagerVM = new AEMacroManagerViewModel(scanner, messageBox, macroManager, windowinit);
            var aESettingVM = new AESettingViewModel(new AESetting(), mtpManager);
            var aeScriptGenerator = new AEScriptGenerator(scriptApply, aESettingVM, messageBox, autoMapper, emulatorToScriptFactory, actionToScriptFactory, dataIO, actionProvider);
            var autoUpdater = new AutoUpdater(messageBox);
            var actionList = new AEActionListViewModel(macroManager, repository, messageBox);
            var savemanager = new AEMacroSaveManagerViewModel(saverepo, macroManager);

            return new AEMG
            {
                DataContext = new AEMGViewModel(macroManagerVM, actionList, aESettingVM, aeScriptGenerator, messageBox, autoUpdater, savemanager, macroManager)
            };
        }

        private void SetupWindowInit(ApplicationSettingViewModel settingVM, out IWindowInitlizer windowinit)
        {
            windowinit = new WindowInit();
            Setting window = null;

            settingVM.SettingSaved += (sender, e) =>
            {
                window?.Close();
            };

            windowinit.SettingWindowOpen += (sender, e) =>
            {
                settingVM.LoadSetting();
                window = new Setting();
                window.DataContext = settingVM;

                window.ShowDialog();
            };
        }
    }
}
