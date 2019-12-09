using Data;
using System;
using System.Collections.Generic;

namespace AEMG_EX.Core
{
    public interface IMacroManager
    {
        /// <summary>
        /// Get the currently selected Macro
        /// </summary>
        IEnumerable<IAEActionViewModel> GetCurrentAEActionList();

        MacroTemplate GetCurrentTemplate();

        /// <summary>
        /// Scan the macro folder
        /// </summary>
        void ScanForMacroes();

        /// <summary>
        /// Fire when macro selection changed
        /// </summary>
        event EventHandler SelectChanged;
    }
}
