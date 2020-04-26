using Data;
using System;
using System.Collections.Generic;
using EMM.Core.ViewModels;
using MTPExplorer;

namespace EMM.Core
{
    /// <summary>
    /// Return instance of script apply base on <see cref="Emulator"/>
    /// </summary>
    public class ScriptApplyFactory
    {
        public ScriptApplyFactory(Dictionary<Emulator, IApplyScriptToFolder> scriptApplyDict)
        {
            this.scriptApplyDict = scriptApplyDict;
        }

        private Dictionary<Emulator, IApplyScriptToFolder> scriptApplyDict;

        /// <summary>
        /// Get the strategy to handle apply to script folder from chosen <see cref="Emulator"/>
        /// </summary>
        /// <param name="emulator">Chosen emulator from <see cref="SettingViewModel"/></param>
        /// <returns></returns>
        public IApplyScriptToFolder GetScriptApplier(Emulator emulator)
        {
            if (!scriptApplyDict.ContainsKey(emulator))
                throw new NotImplementedException("This script apply is not implement");

            return scriptApplyDict[emulator];
        }
    }

    public class ScriptApplyBootStrap
    {
        public ScriptApplyBootStrap(IMessageBoxService messageBoxService, IMTPManager mTPManager)
        {
            this.messageBoxService = messageBoxService;
            this.mTPManager = mTPManager;
        }

        private IMessageBoxService messageBoxService;
        private IMTPManager mTPManager;

        private Dictionary<Emulator, IApplyScriptToFolder> scriptApplyDict;

        public ScriptApplyFactory GetScriptApplyFactory()
        {
            if (scriptApplyDict == null) {
                scriptApplyDict = new Dictionary<Emulator, IApplyScriptToFolder>()
                {
                    { Emulator.Nox, new NoxScriptApply(messageBoxService, mTPManager) },
                    { Emulator.Memu, new MemuScriptApply(messageBoxService, mTPManager) },
                    { Emulator.BlueStack, new BlueStackScriptApply(messageBoxService, mTPManager) },
                    { Emulator.LDPlayer, new LDPlayerScriptApply(messageBoxService, mTPManager) },
                    { Emulator.HiroMacro, new HiroMacroScriptApply(messageBoxService, mTPManager) },
                    { Emulator.AnkuLua, new AnkuMacroScriptApply(messageBoxService, mTPManager) },
                    { Emulator.AutoTouch, new AnkuMacroScriptApply(messageBoxService, mTPManager) },
                    { Emulator.Robotmon, new RobotmonScriptApply(messageBoxService, mTPManager) },
                };
            }

            return new ScriptApplyFactory(scriptApplyDict);
        }
    }
}
