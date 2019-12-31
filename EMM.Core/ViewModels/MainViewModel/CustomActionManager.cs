using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Data;
using EMM.Core.ViewModels;

namespace EMM.Core
{
    /// <summary>
    /// ViewModel for managing custom Action
    /// </summary>
    public class CustomActionManager : BaseViewModel
    {
        #region Ctor

        public CustomActionManager(MacroManagerViewModel macroManager, ViewModelFactory viewModelFactory, DataIO dataIO, IMessageBoxService messageBoxService)
        {
            this.macroManager = macroManager;
            this.viewModelFactory = viewModelFactory;
            this.dataIO = dataIO;
            this.messageBoxService = messageBoxService;

            LoadDefault();
            LoadCustomActionList(this.savedCustomAction);
            InitializeCommands();
        } 

        #endregion

        #region Private members

        private MacroManagerViewModel macroManager;
        private ViewModelFactory viewModelFactory;
        private DataIO dataIO;
        private IMessageBoxService messageBoxService;

        private readonly string savedCustomAction = Path.Combine(Environment.CurrentDirectory, "Setting", "SavedActions");
        private readonly string defaultCustomAction = Path.Combine(Environment.CurrentDirectory, "Setting", "DefaultSavedActions");

        #endregion

        #region Public Properties

        /// <summary>
        /// The currently selected <see cref="ActionGroupViewModel"/>
        /// </summary>
        private ActionGroupViewModel selectedActionGroupViewModel => GetSelectedActionGroupViewModel();

        /// <summary>
        /// List of <see cref="ActionGroupViewModel"/> which is Custom Action in this case
        /// </summary>
        public ObservableCollection<ActionGroupViewModel> CustomActionList { get; set; }

        /// <summary>
        /// To open new custom action pop up
        /// </summary>
        public bool IsCreateNewCustomAction { get; set; }

        /// <summary>
        /// Name of the custom action to be created
        /// </summary>
        public string NewCustomActionName { get; set; } = string.Empty;

        #endregion

        #region Commands   

        public ICommand CreateCustomActionCommand { get; set; }
        public ICommand InsertSavedCustomActionCommand { get; set; }
        public ICommand DeleteCustomActionCommand { get; set; }
        public ICommand OpenCustomActionPopupCommand { get; set; }
        public ICommand SaveCustomActionCommand { get; set; }

        public ICommand UnPackCustomActionCommand { get; set; }
        public ICommand ResetToDefaultCommand { get; set; }

        /// <summary>
        /// Initilize all commands in this viewmodel
        /// </summary>
        private void InitializeCommands()
        {
            OpenCustomActionPopupCommand = new RelayCommand(p =>
            {
                IsCreateNewCustomAction ^= true;
            });

            CreateCustomActionCommand = new RelayCommand(p =>
            {            
                if (NewCustomActionName.Equals(string.Empty))
                    return;

                if (this.selectedActionGroupViewModel == null)
                    return;

                var selectedActionVM = this.selectedActionGroupViewModel.ViewModelList.GetSelectedElement();

                if (selectedActionVM.Count == 0)
                    return;

                var customAction = (ActionGroupViewModel)viewModelFactory.NewActionViewModel(BasicAction.ActionGroup);

                customAction.ActionDescription = NewCustomActionName;
                foreach (var actionVM in selectedActionVM)
                    customAction.ViewModelList.Add(actionVM.MakeCopy());

                if (CustomActionList == null)
                    CustomActionList = new ObservableCollection<ActionGroupViewModel>();

                CustomActionList.Add(customAction);

                SaveCustomActionList();

                //remove actionVM, insert newly created custom action
                var index = this.selectedActionGroupViewModel.ViewModelList.IndexOf(selectedActionVM[0]);
                foreach (var actionVM in selectedActionVM)
                    this.selectedActionGroupViewModel.ViewModelList.Remove(actionVM);

                this.selectedActionGroupViewModel.ViewModelList.Insert(index, customAction.MakeCopy());

                OpenCustomActionPopupCommand.Execute(null);

            }, p => selectedActionGroupViewModel != null);

            InsertSavedCustomActionCommand = new RelayCommand(p =>
            {
                if (!(p is ActionGroupViewModel selectedCustomActionVM))
                    return;

                if (this.selectedActionGroupViewModel == null)
                    return;

                var index = this.selectedActionGroupViewModel.SelectedItemIndex;

                this.selectedActionGroupViewModel.ViewModelList.Insert(index + 1, selectedCustomActionVM.MakeCopy());
            }, p => selectedActionGroupViewModel != null);

            DeleteCustomActionCommand = new RelayCommand(p =>
            {
                if (!(p is ActionGroupViewModel selectedCustomActionVM))
                    return;

                this.CustomActionList.Remove(selectedCustomActionVM);

                SaveCustomActionList();
            });

            SaveCustomActionCommand = new RelayCommand(p =>
            {
                if (selectedActionGroupViewModel == null)
                    return;

                var selectedCustomAction = this.selectedActionGroupViewModel.SelectedItem;

                if (!(selectedCustomAction is ActionGroupViewModel))
                    return;

                this.CustomActionList.Add(selectedCustomAction.MakeCopy() as ActionGroupViewModel);

                SaveCustomActionList();           
            }, p => selectedActionGroupViewModel != null);

            UnPackCustomActionCommand = new RelayCommand(p =>
            {
                if (this.selectedActionGroupViewModel == null)
                    return;

                //get the current selected action, check if it's an actiongroupviewmodel
                var selectedActions = this.selectedActionGroupViewModel.ViewModelList.GetSelectedElement();

                if (!(selectedActions.Count > 0))
                    return;

                var selectedCustomAction = selectedActions[0] as ActionGroupViewModel;

                if (selectedCustomAction == null)
                    return;

                //remove the current selected custom action
                var index = this.selectedActionGroupViewModel.ViewModelList.IndexOf(selectedCustomAction);
                this.selectedActionGroupViewModel.ViewModelList.RemoveAt(index);

                //insert back the list
                foreach (var actionViewModel in selectedCustomAction.ViewModelList)
                {
                    this.selectedActionGroupViewModel.ViewModelList.Insert(index, actionViewModel);
                    index++;
                }
            }, p => selectedActionGroupViewModel != null);

            ResetToDefaultCommand = new RelayCommand(p =>
            {
                //Delete user file then load default
                if (File.Exists(this.savedCustomAction))
                    File.Delete(this.savedCustomAction);

                this.CustomActionList.Clear();
                LoadDefault();
                LoadCustomActionList(this.savedCustomAction);
            });
        }

        #endregion

        private void SaveCustomActionList()
        {
            var actionGroupListToBeSave = new List<IAction>();
            actionGroupListToBeSave.AddRange(CustomActionList.Select(i => i.ConvertBackToAction()));

            dataIO.SaveCustomActions(actionGroupListToBeSave, this.savedCustomAction);
        }

        private ActionGroupViewModel GetSelectedActionGroupViewModel()
        {
            if (this.macroManager.CurrentMacro == null)
                return null;

            var selectedActionGroupVMList = this.macroManager.CurrentMacro.ViewModelList.GetSelectedElement();

            if (selectedActionGroupVMList.Count == 0)
                return null;

            return (ActionGroupViewModel)selectedActionGroupVMList[0];
        }

        /// <summary>
        /// Load custom actionviewmodel from path to saved action
        /// </summary>
        /// <param name="path"></param>
        private void LoadCustomActionList(string path)
        {
            var actionGroupList = this.dataIO.LoadCustomActions(path);

            if (CustomActionList == null)
                CustomActionList = new ObservableCollection<ActionGroupViewModel>();

            if (actionGroupList == null)
                return;

            foreach (var actionGroup in actionGroupList)
            {
                var viewmodel = this.viewModelFactory.NewActionViewModel(actionGroup.BasicAction).ConvertFromAction(actionGroup);
                if (viewmodel == null)
                    continue;
                CustomActionList.Add(viewmodel as ActionGroupViewModel);
            }
        }

        private void LoadDefault()
        {
            //if the file SavedActions is not found then copy from the default action file
            if (!File.Exists(this.savedCustomAction))
                if (File.Exists(this.defaultCustomAction))
                {
                    File.Copy(this.defaultCustomAction, this.savedCustomAction);
                }
        }
    }
}
