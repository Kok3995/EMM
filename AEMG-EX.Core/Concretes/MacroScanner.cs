using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;

namespace AEMG_EX.Core
{
    public class MacroScanner : IScanner
    {
        public MacroScanner(DataIO dataIO, IAEActionFactory factory)
        {
            this.dataIO = dataIO;
            this.factory = factory;
        }

        private DataIO dataIO;
        private IAEActionFactory factory;

        public IEnumerable<LoadedTemplate> ScanAll(string path)
        {
            //Create path if not exist
            Directory.CreateDirectory(path);

            var files = Directory.EnumerateFiles(path);

            //Check empty
            if (!files.Any())
                yield break;

            foreach (var file in files)
            {
                yield return this.dataIO.LoadMacroFileFromPath(file);
            }
        }

        public IEnumerable<IAEActionViewModel> ScanMacroForPlaceHolder(MacroTemplate macro)
        {
            for (int i = 0; i < macro.ActionGroupList.Count; i++)
            {
                if (!(macro.ActionGroupList[i] is ActionGroup actionGroup))
                    throw new InvalidOperationException("Can not cast ActionGroup");

                if (actionGroup.IsDisable)
                    continue;


                for (int j = 0; j < actionGroup.ActionList.Count; j++)
                {
                    if (!(actionGroup.ActionList[j] is AE ae) || ae.IsDisable)
                        continue;

                    var aeaction = this.factory.NewAEActionViewModel(ae.AnotherEdenAction);
                    aeaction.ActionDescription = ae.ActionDescription;
                    aeaction.ActionGroupIndex = i;
                    aeaction.ActionIndex = j;
                    yield return aeaction;
                }
            }
        }

        public LoadedTemplate ScanSingle(string path)
        {
            return dataIO.LoadMacroFileFromPath(path);
        }

        public IEnumerable<LoadedTemplate> ScanUserSelected(Action<Newtonsoft.Json.Serialization.ErrorEventArgs> errorCallback = null)
        {
            return dataIO.LoadFromFileMultiple(AEMGStatic.MACRO_FOLDER, errorCallback);
        }
    }
}
