using Data;
using System.Collections.Generic;

namespace AEMG_EX.Core
{
    public interface IScanner
    {
        /// <summary>
        /// Scan for Macroes in folder
        /// </summary>
        /// <param name="path">The folder to scan</param>
        /// <returns></returns>
        IEnumerable<LoadedTemplate> ScanAll(string path);

        /// <summary>
        /// Scan the macro with given path
        /// </summary>
        /// <param name="path">Path to the macro</param>
        /// <returns></returns>
        LoadedTemplate ScanSingle(string path);

        /// <summary>
        /// Open file dialog and Scan all the macro the user selected
        /// </summary>
        /// <returns></returns>
        IEnumerable<LoadedTemplate> ScanUserSelected();

        IEnumerable<IAEActionViewModel> ScanMacroForPlaceHolder(MacroTemplate macro);
    }
}
