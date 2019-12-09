using AEMG_EX.Core;
using Data;
using EMM.Core;
using EMM.Core.Converter;
using EMM.Core.Service;
using EMM.Core.Update;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AEMG_EX
{
    public class Bootstrap
    {
        public AEMG SetupMainWindow()
        {
            //reset working directory
            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var dataIO = new DataIO();
            var aeActionFactory = new AEActionFactory();
            var autoMapper = new SimpleAutoMapper();
            var messageBox = new MessageBoxService();

            var scriptApplyDict = new Dictionary<Emulator, IApplyScriptToFolder>()
            {
                { Emulator.Nox, new NoxScriptApply(messageBox) },
                { Emulator.Memu, new MemuScriptApply(messageBox) }
            };

            var scriptApply = new ScriptApplyFactory(messageBox, scriptApplyDict);
            var scanner = new MacroScanner(dataIO, aeActionFactory);
            var macroManager = new AEMacroManager(scanner, messageBox);
            var actionList = new AEActionListViewModel(macroManager);
            var aESettingVM = new AESettingViewModel(new AESetting());
            var aeScriptGenerator = new AEScriptGenerator(scriptApply, aESettingVM, messageBox, autoMapper);
            var autoUpdater = new AutoUpdater(messageBox);

            return new AEMG
            {
                DataContext = new AEMGViewModel(macroManager, actionList, aESettingVM, aeScriptGenerator, messageBox, autoUpdater)
            };
        }
    }
}
