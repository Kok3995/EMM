using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    /// <summary>
    /// Manage ae save setup
    /// </summary>
    public interface IAESavedSetupRepository
    {
        /// <summary>
        /// Current save
        /// </summary>
        /// <returns></returns>
        SavedAESetup CurrentSave();

        /// <summary>
        /// Get macro's save data by its path
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        SavedAESetup FindByMacroPath(string path);

        /// <summary>
        /// Get macro's save data by its name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        SavedAESetup FindByMacroName(string fileName);

        /// <summary>
        /// Add new profile
        /// </summary>
        /// <param name="profile"></param>
        void AddProfile(SavedAEProfile profile);

        /// <summary>
        /// Remove a profile
        /// </summary>
        /// <param name="profile"></param>
        void RemoveProfile(SavedAEProfile profile);

        /// <summary>
        /// Save changes to disk
        /// </summary>
        /// <returns></returns>
        bool SaveChange();

        /// <summary>
        /// Reset default of every profile
        /// </summary>
        void ResetProfileDefault();
    }
}
