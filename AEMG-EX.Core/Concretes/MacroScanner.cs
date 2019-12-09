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
                var actionGroup = macro.ActionGroupList[i] as ActionGroup;
                if (actionGroup == null)
                    throw new InvalidOperationException("Can not cast ActionGroup");


                for (int j = 0; j < actionGroup.ActionList.Count; j++)
                {
                    var ae = actionGroup.ActionList[j] as AE;
                    if (ae == null)
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

        public IEnumerable<LoadedTemplate> ScanUserSelected()
        {
            return dataIO.LoadFromFileMultiple(AEMGStatic.MACRO_FOLDER);
        }
    }
}
