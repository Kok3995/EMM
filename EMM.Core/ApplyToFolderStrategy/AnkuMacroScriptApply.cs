using Data;
using MTPExplorer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EMM.Core
{
    /// <summary>
    /// This class is used to apply script to emulator folder
    /// </summary>
    public class AnkuMacroScriptApply : HiroMacroScriptApply
    {
        public AnkuMacroScriptApply(IMessageBoxService messageBoxService, IMTPManager mTPManager) : base(messageBoxService, mTPManager)
        {
        }

        protected override string FileExtension => ".lua";

        public override bool? ApplyScriptTo(string scriptName, string path, object scriptObj, bool prompt = true)
        {
            var fullScript = $"setManualTouchParameter(99999, 12)\n" +
                $"actions = {{ {scriptObj} }}\n" +
                $"manualTouch(actions)";

            return base.ApplyScriptTo(scriptName, path, fullScript, prompt);
        }
    }
}