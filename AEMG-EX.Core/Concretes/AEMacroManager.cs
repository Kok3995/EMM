using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace AEMG_EX.Core
{
    public class AEMacroManager : IMacroManager
    {
        public AEMacroManager(IScanner scanner)
        {
            this.scanner = scanner;
        }

        private MacroTemplate selectedMacro;
        private IScanner scanner;
        private Dictionary<MacroTemplate, List<IAEActionViewModel>> cache;
        private bool isFromCache;

        public event EventHandler<MacroSelectionChangedEventArgs> SelectChanged;
        public event EventHandler BeforeMacroReloaded;
        public event EventHandler AfterMacroReloaded;
        public event EventHandler<MacroProfileSelectedEventArgs> MacroProfileSelected;
        public event EventHandler<MacroSaveLoadedEventArgs> MacroSaveLoaded;

        public List<IAEActionViewModel> GetCurrentAEActionList()
        {
            if (this.selectedMacro == null)
                return null;

            //check cache
            if (this.cache == null)
                this.cache = new Dictionary<MacroTemplate, List<IAEActionViewModel>>();

            if (this.cache.ContainsKey(this.selectedMacro))
            {
                isFromCache = true;
                return this.cache[this.selectedMacro];
            }

            var aevmList = this.scanner.ScanMacroForPlaceHolder(this.selectedMacro).ToList();

            this.cache[this.selectedMacro] = aevmList;
            isFromCache = false;
            return aevmList;
        }

        public MacroTemplate GetCurrentTemplate()
        {
            return selectedMacro;
        }

        public void SetCurrentTemplate(MacroTemplate macro, string path)
        {
            var old = selectedMacro;
            selectedMacro = macro;

            SelectChanged?.Invoke(this, new MacroSelectionChangedEventArgs(old, selectedMacro, path));
        }

        public void ClearCache()
        {
            if (this.cache == null)
                return;

            this.cache.Clear();
        }

        public void InvokeBeforeMacroReloaded()
        {
            this.BeforeMacroReloaded?.Invoke(null, EventArgs.Empty);
        }

        public void InvokeAfterMacroReloaded()
        {
            this.AfterMacroReloaded?.Invoke(null, EventArgs.Empty);
        }

        public void InvokeLoadMacroProfile(SavedAEProfile profile)
        {
            this.MacroProfileSelected?.Invoke(null, new MacroProfileSelectedEventArgs(selectedMacro, profile));
        }

        public void InvokeSaveLoaded(SavedAESetup save)
        {
            if (!isFromCache)
                this.MacroSaveLoaded?.Invoke(null, new MacroSaveLoadedEventArgs(save));
        }
    }



    public class MacroSelectionChangedEventArgs : EventArgs
    {
        public MacroSelectionChangedEventArgs(MacroTemplate oldMacro, MacroTemplate newMacro, string path)
        {
            OldMacro = oldMacro;
            NewMacro = newMacro;
            SelectedMacroPath = path;
        }

        public MacroTemplate NewMacro { get; set; }

        public MacroTemplate OldMacro { get; set; }

        public string SelectedMacroPath { get; set; }
    }

    public class MacroProfileSelectedEventArgs : EventArgs
    {
        public MacroProfileSelectedEventArgs(MacroTemplate currentTemplate, SavedAEProfile profile)
        {
            Macro = currentTemplate;
            Profile = profile;
        }

        public MacroTemplate Macro { get; set; }


        public SavedAEProfile Profile { get; set; }
    }

    public class MacroSaveLoadedEventArgs : EventArgs
    {
        public MacroSaveLoadedEventArgs(SavedAESetup save)
        {
            Save = save;
        }

        public SavedAESetup Save { get; set; }
    }
}
