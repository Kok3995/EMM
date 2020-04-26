using Data;
using System;
using System.Collections.Generic;

namespace AEMG_EX.Core
{
    public interface IMacroManager
    {
        /// <summary>
        /// Get the currently selected Macro aeaction list, return null if selected macro is null
        /// </summary>
        List<IAEActionViewModel> GetCurrentAEActionList();

        MacroTemplate GetCurrentTemplate();

        /// <summary>
        /// Set the currently selected macro
        /// </summary>
        void SetCurrentTemplate(MacroTemplate macro, string path);

        /// <summary>
        /// Fire when macro selection changed
        /// </summary>
        event EventHandler<MacroSelectionChangedEventArgs> SelectChanged;

        /// <summary>
        /// Fire when a macro profile is selected
        /// </summary>
        event EventHandler<MacroProfileSelectedEventArgs> MacroProfileSelected;

        /// <summary>
        /// Fire after macro save loaded, will not fire if macro is load from cache
        /// </summary>
        event EventHandler<MacroSaveLoadedEventArgs> MacroSaveLoaded;

        /// <summary>
        /// Fire before beginning to reload macro
        /// </summary>
        event EventHandler BeforeMacroReloaded;

        /// <summary>
        /// Fire when macro reloaded
        /// </summary>
        event EventHandler AfterMacroReloaded;

        /// <summary>
        /// Invoke before macro reload
        /// </summary>
        void InvokeBeforeMacroReloaded();

        /// <summary>
        /// Invoke after macro is reloaded
        /// </summary>
        void InvokeAfterMacroReloaded();

        /// <summary>
        /// Invoke after macro profile is selected
        /// </summary>
        void InvokeLoadMacroProfile(SavedAEProfile profile);

        /// <summary>
        /// Invoke after save loaded
        /// </summary>
        /// <param name="save"></param>
        void InvokeSaveLoaded(SavedAESetup save);

        /// <summary>
        /// Clear macro cache
        /// </summary>
        void ClearCache();
    }
}
