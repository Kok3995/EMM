using EMM;
using EMM.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AEMG_EX.Core
{
    public class AEActionListViewModel : BaseViewModel
    {
        #region Ctor

        public AEActionListViewModel(IMacroManager macroManager, IAERepository repository, IMessageBoxService messageBoxService)
        {
            this.macroManager = macroManager;
            this.repository = repository;
            this.messageBoxService = messageBoxService;

            AEActionList = new ObservableCollection<IAEActionViewModel>();
            SavedAEActions = new ObservableCollection<IAEAction>(repository.FindAll());

            InitializeCommandAndEvents();
        }

        #endregion

        #region Private members

        private readonly IMacroManager macroManager;

        private IAEActionViewModel copyAEAction;

        private IAERepository repository;

        private IMessageBoxService messageBoxService;

        private List<IAEActionViewModel> oldList;

        #endregion

        #region Public Properties

        public ObservableCollection<IAEActionViewModel> AEActionList { get; set; }

        public IAEActionViewModel SelectedAEAction { get; set; }

        public ObservableCollection<IAEAction> SavedAEActions { get; set; }

        public bool IsSavedSetupDialogOpen { get; set; }

        public string SavedSetupName { get; set; }

        #endregion

        #region Commands

        public ICommand CopyCommand { get; set; }
        public ICommand ApplyCommand { get; set; }

        public ICommand OpenSavedDialogCommand { get; set; }
        public ICommand CloseSavedDialogCommand { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        private void InitializeCommandAndEvents()
        {
            CopyCommand = new RelayCommand(p =>
            {
                this.copyAEAction = this.SelectedAEAction;
            }, p => SelectedAEAction != null);

            ApplyCommand = new RelayCommand(p =>
            {
                foreach (var action in AEActionList.GetSelectedElement())
                {
                    if (action.AEAction == copyAEAction.AEAction)
                        action.CopyOntoSelf(copyAEAction);
                };
            }, p => this.copyAEAction != null);

            OpenSavedDialogCommand = new RelayCommand(p =>
            {
                IsSavedSetupDialogOpen = true;
            }, p => SelectedAEAction != null);

            CloseSavedDialogCommand = new RelayCommand(p =>
            {
                IsSavedSetupDialogOpen = false;
            }, p => IsSavedSetupDialogOpen == true);

            SaveCommand = new RelayCommand(p =>
            {
                var action = this.SelectedAEAction.ConvertBackToAction();
                action.Name = SavedSetupName;
                repository.Add(action);

                if (!repository.SaveChange())
                    messageBoxService.ShowMessageBox("Can not save your setup. Check the permission to the folder", "ERROR", MessageButton.OK, MessageImage.Error);
                else
                {
                    SavedAEActions.Add(action);
                    IsSavedSetupDialogOpen = false;
                }

            }, p => SelectedAEAction != null && !string.IsNullOrWhiteSpace(SavedSetupName));

            LoadCommand = new RelayCommand(p =>
            {
                if (!(p is IAEAction aeAction))
                    return;

                foreach (var action in AEActionList.GetSelectedElement())
                {
                    if (action.AEAction == aeAction.AEAction)
                        action.ConvertFromAction(aeAction);
                };
            }, p => SavedAEActions.Count > 0);

            RemoveCommand = new RelayCommand(p =>
            {
                if (!(p is IAEAction aeAction))
                    return;

                repository.Remove(aeAction);

                if (!repository.SaveChange())
                    messageBoxService.ShowMessageBox("Can not remove the saved setup. Check the permission to the folder", "ERROR", MessageButton.OK, MessageImage.Error);
                else
                {
                    SavedAEActions?.Remove(aeAction);
                }

            }, p => SavedAEActions.Count > 0);

            this.macroManager.SelectChanged += (sender, e) =>
            {
                var newList = macroManager.GetCurrentAEActionList();

                AEActionList.Clear();

                if (newList != null)
                {
                    foreach (var a in newList)
                    {
                        AEActionList.Add(a);
                    }
                }

                this.copyAEAction = null;
            };

            this.macroManager.BeforeMacroReloaded += (sender, e) =>
            {
                oldList = new List<IAEActionViewModel>(AEActionList);
            };

            this.macroManager.AfterMacroReloaded += (sender, e) =>
            {
                if (oldList != null)
                {
                    var count = oldList.Count < AEActionList.Count ? oldList.Count : AEActionList.Count;

                    for (int i = 0; i < count; i++)
                    {
                        AEActionList[i].CopyOntoSelf(oldList[i]);
                    }
                }
            };

            this.macroManager.MacroProfileSelected += (sender, e) =>
            {
                if (e == null || e.Profile.Setups?.Count == 0) 
                    return;

                ApplyProfile(e.Profile);
            };

            this.macroManager.MacroSaveLoaded += (sender, e) =>
            {
                if (e == null || e.Save == null)
                    return;

                ApplyProfile(e.Save.DefaultProfile);
            };
        }

        #endregion

        public IAEActionViewModel GetSelected()
        {
            return this.SelectedAEAction;
        }

        private void ApplyProfile(SavedAEProfile profile)
        {
            if (profile == null) 
                return;

            var count = profile.Setups.Count < AEActionList.Count ? profile.Setups.Count : AEActionList.Count;

            for (int i = 0; i < count; i++)
            {
                AEActionList[i].ConvertFromAction(profile.Setups[i]);
            }
        }
    }
}
