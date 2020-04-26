using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AEMG_EX.Core
{
    public class AESavedSetupRepository : IAESavedSetupRepository
    {
        public AESavedSetupRepository(string path)
        {
            Refresh(path);
        }

        public AESavedSetupRepository()
        {

        }

        private SavedAESetup savedAESetup;

        public SavedAESetup CurrentSave()
        {
            return savedAESetup;
        }

        public SavedAESetup FindByMacroPath(string path)
        {
            Refresh(path);

            return savedAESetup;
        }

        public SavedAESetup FindByMacroName(string fileName)
        {
            return FindByMacroPath(BuildSaveFilePath(fileName));
        }

        public void AddProfile(SavedAEProfile profile)
        {
            if (savedAESetup == null || profile == null)
                return;

            savedAESetup.Profiles.Add(profile);
        }

        public void RemoveProfile(SavedAEProfile profile)
        {
            if (savedAESetup == null || profile == null)
                return;

            savedAESetup.Profiles.Remove(profile);
        }

        public bool SaveChange()
        {
            if (savedAESetup == null)
                return false;

            try
            {
                File.WriteAllText(BuildSaveFilePath(savedAESetup.MacroFileName),JsonConvert.SerializeObject(savedAESetup, Formatting.Indented));

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Refresh(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            var fileName = Path.GetFileNameWithoutExtension(path);
            var savedFilePath = BuildSaveFilePath(fileName);

            if (!File.Exists(savedFilePath))
            {
                savedAESetup = new SavedAESetup
                {
                    MacroFileName = fileName,
                    Profiles = new List<SavedAEProfile>(),
                };

                SaveChange();
            }

            if (savedAESetup == null || !savedAESetup.MacroFileName.Equals(fileName))
            {
                savedAESetup = JsonConvert.DeserializeObject<SavedAESetup>(File.ReadAllText(BuildSaveFilePath(fileName)), new CustomIAEActionJsonConverter());
                if (savedAESetup != null)
                    savedAESetup.MacroFileName = fileName;
            }
        }

        private string BuildSaveFilePath(string fileName)
        {
            return Path.Combine(AEMGStatic.MACRO_FOLDER, fileName + AEMGStatic.SAVED_SETUP_EXTENSION);
        }

        public void ResetProfileDefault()
        {
            if (savedAESetup == null)
                return;

            savedAESetup.Profiles.ForEach(s => s.IsDefault = false);
        }
    }
}
