using Data;
using System;
using System.Collections.Generic;
using EMM.Core.ViewModels;

namespace EMM.Core
{
    /// <summary>
    /// Return instance of script apply base on <see cref="Emulator"/>
    /// </summary>
    public class ScriptApplyFactory
    {
        public ScriptApplyFactory(IMessageBoxService messageBoxService, Dictionary<Emulator, IApplyScriptToFolder> scriptApplyDict)
        {
            this.messageBoxService = messageBoxService;
            this.scriptApplyDict = scriptApplyDict;
        }

        private IMessageBoxService messageBoxService;

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
}
