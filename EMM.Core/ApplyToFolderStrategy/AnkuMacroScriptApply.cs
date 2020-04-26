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
    }
}