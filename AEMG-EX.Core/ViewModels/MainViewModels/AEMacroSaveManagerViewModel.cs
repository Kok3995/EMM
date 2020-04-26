using EMM;
using EMM.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AEMG_EX.Core
{
    public class AEMacroSaveManagerViewModel : BaseViewModel
    {
        public AEMacroSaveManagerViewModel(IAESavedSetupRepository repository, IMacroManager manager)
        {
            this.repository = repository;
            this.manager = manager;

            InitializeCommandsAndEvents();
        }

        private IAESavedSetupRepository repository;
        private IMacroManager manager;

        public SavedAESetup Save { get; set; }

        public ObservableCollection<SavedAEProfile> Profiles { get; set; }

        public string NewProfileName { get; set; }

        public bool IsCreateNewProfile { get; set; }

        public ICommand NewProfileCommand { get; set; }
        public ICommand RemoveProfileCommand { get; set; }
        public ICommand LoadProfileCommand { get; set; }
        public ICommand OpenProfilePopupCommand { get; set; }
        public ICommand UpdateCurrentProfileCommand { get; set; }
        

        private void InitializeCommandsAndEvents()
        {
            OpenProfilePopupCommand = new RelayCommand(p =>
            {
                IsCreateNewProfile ^= true;
            }, p => Save != null);

            NewProfileCommand = new RelayCommand(p =>
            {
                var currentTemplate = manager.GetCurrentTemplate();

                if (string.IsNullOrWhiteSpace(NewProfileName) || !AEMGHelpers.IsValidFilename(NewProfileName) || currentTemplate == null)
                    return;

                var newProfile = new SavedAEProfile
                {
                    Id = Guid.NewGuid().ToString(),
                    IsDefault = true,
                    Name = NewProfileName,
                    Setups = GetCurrentSetup(),
                };

                repository.ResetProfileDefault();
                repository.AddProfile(newProfile);
                repository.SaveChange();

                RefreshSave();
                IsCreateNewProfile = false;
            }, p => Save != null);

            RemoveProfileCommand = new RelayCommand(p =>
            {
                if (!(p is SavedAEProfile save))
                    return; 

                repository.RemoveProfile(save);
                repository.SaveChange();

                RefreshSave();
            }, p => Save != null);

            LoadProfileCommand = new RelayCommand(p =>
            {
                if (!(p is SavedAEProfile save))
                    return;

                repository.ResetProfileDefault();
                save.IsDefault = true;
                repository.SaveChange();

                manager.InvokeLoadMacroProfile(save);
            }, p => Save != null);

            UpdateCurrentProfileCommand = new RelayCommand(p =>
            {
                if (repository.CurrentSave()?.DefaultProfile == null)
                    return;

                var oldSetups = repository.CurrentSave().DefaultProfile.Setups;
                var currentSetups = GetCurrentSetup();

                //merge setups
                if (currentSetups.Count >= oldSetups.Count)
                {
                    repository.CurrentSave().DefaultProfile.Setups = currentSetups;
                }
                else
                {
                    for (int i = 0; i < currentSetups.Count; i++)
                    {
                        if (i < oldSetups.Count)
                            oldSetups[i] = currentSetups[i];
                    }
                }

                repository.SaveChange();
            }, p => repository.CurrentSave()?.DefaultProfile != null);

            manager.SelectChanged += (sender, test) =>
            {
                if (test.NewMacro == null)
                {
                    Save = null;
                    Profiles?.Clear();
                    return;
                }

                Save = repository.FindByMacroPath(test.SelectedMacroPath);
                Profiles = new ObservableCollection<SavedAEProfile>(Save.Profiles);

                //Fire event to load default save profile
                if (Profiles.Count > 0)
                {
                    manager.InvokeSaveLoaded(repository.CurrentSave());
                }
            };
        }

        private List<IAEAction> GetCurrentSetup()
        {
            var list = new List<IAEAction>();
            var currentList = manager.GetCurrentAEActionList();

            if (currentList != null)
            {
                foreach (var setup in currentList)
                {
                    list.Add(setup.ConvertBackToAction());
                }
            }

            return list; 
        }

        private void RefreshSave()
        {
            if (Save == null)
                return;

            Save = repository.FindByMacroName(Save.MacroFileName);
            Profiles = new ObservableCollection<SavedAEProfile>(Save.Profiles);
        }
    }
}
