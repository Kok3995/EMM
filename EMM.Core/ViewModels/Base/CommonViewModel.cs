using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EMM.Core.ViewModels
{
    /// <summary>
    /// Inherit this class to give viewmodel ability to add, insert, copy....
    /// </summary>
    public abstract class CommonViewModel : BaseViewModel
    {
        public CommonViewModel()
        {
            InitializeCommands();
        }

        #region Private members

        /// <summary>
        /// Use to undo cut command, save a new backup list to set back
        /// </summary>
        private ObservableCollection<IActionViewModel> backupCutList;

        /// <summary>
        /// List of item to be copied <see cref="IActionViewModel"/>
        /// </summary>
        private List<IActionViewModel> copiedList = new List<IActionViewModel>();

        #endregion

        #region Public Properties

        /// <summary>
        /// List of action group
        /// </summary>
        public ObservableCollection<IActionViewModel> ViewModelList { get; set; } = new ObservableCollection<IActionViewModel>();

        /// <summary>
        /// The selected item
        /// </summary>
        public IActionViewModel SelectedItem { get; set; }

        /// <summary>
        /// The index of the selected group, default to -1 so insert work first time
        /// </summary>
        public int SelectedItemIndex { get; set; } = -1;

        /// <summary>
        /// Indicate if cut command is used previously
        /// </summary>
        public bool IsCut { get; set; }

        /// <summary>
        /// Disable action list part when no action group selected or created
        /// </summary>
        public bool IsActionEnable => (SelectedItem == null) ? false : true;

        #endregion

        #region Commands

        public ICommand AddCommand { get; set; }
        public ICommand InsertCommand { get; set; }
        public ICommand CopyCommand { get; set; }
        public ICommand CutCommand { get; set; }
        public ICommand UndoCutCommand { get; set; }
        public ICommand PasteCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand DownCommand { get; set; }

        /// <summary>
        /// Initilize all commands in this viewmodel
        /// </summary>
        private void InitializeCommands()
        {
            AddCommand = new RelayCommand(AddItem);
            InsertCommand = new RelayCommand(InsertItem);
            CopyCommand = new RelayCommand(CopyItem, p => SelectedItem != null);
            CutCommand = new RelayCommand(CutItem, p => SelectedItem != null);
            UndoCutCommand = new RelayCommand(UndoCutItem);
            PasteCommand = new RelayCommand(PasteItem, p => this.copiedList.Count > 0);
            DeleteCommand = new RelayCommand(DeleteItem, p => this.SelectedItem != null);
            UpCommand = new RelayCommand(MoveItemUp, p => SelectedItem != null);
            DownCommand = new RelayCommand(MoveItemDown, p => SelectedItem != null);
        }
        #endregion

        #region Commands Implement

            /// <summary>
            /// Override this method to implement add item to list of <see cref="IActionViewModel"/>
            /// </summary>
            /// <param name="parameter"></param>
        protected abstract void AddItem(object parameter);

        /// <summary>
        /// Override this method to implement insert item to list of <see cref="IActionViewModel"/>
        /// </summary>
        /// <param name="parameter"></param>
        protected abstract void InsertItem(object parameter);

        protected virtual void CopyItem(object parameter)
        {
            var selectedItems = this.ViewModelList.GetSelectedElement();

            this.copiedList = selectedItems;

            this.IsCut = false;
        }
        protected virtual void CutItem(object parameter)
        {
            this.CopyItem(null);

            this.backupCutList = new ObservableCollection<IActionViewModel>(this.ViewModelList);

            this.IsCut = true;
        }
        protected virtual void UndoCutItem(object parameter)
        {
            throw new NotImplementedException();
        }
        protected virtual void PasteItem(object parameter)
        {           
            int pasteIndex = this.ViewModelList.GetPasteIndex(SelectedItemIndex);
           
            for (int i = 1; i <= this.copiedList.Count; i++)
            {
                var newItem = this.copiedList[i - 1].MakeCopy();

                this.ViewModelList.Insert(pasteIndex + i, newItem);
            }

            if (this.IsCut == true)
            {
                foreach (var group in this.copiedList)
                    this.ViewModelList.Remove(group);

                this.copiedList.Clear();
                this.IsCut = false;
            }
        }
        protected virtual void DeleteItem(object parameter)
        {
            var currentIndex = this.SelectedItemIndex;

            var selectedItems = this.ViewModelList.GetSelectedElement();

            foreach (var group in selectedItems)
            {
                this.ViewModelList.Remove(group);
            }

            //set the selected index to the deleted index when in the middle of the list, otherwise set it to the previous index
            this.SelectedItemIndex = (currentIndex < this.ViewModelList.Count) ? currentIndex : currentIndex - 1;
        }
        protected virtual void MoveItemUp(object parameter)
        {
            this.ViewModelList.MoveSelectedElement(-1); 
        }
        protected virtual void MoveItemDown(object parameter)
        {
            this.ViewModelList.MoveSelectedElement(1);
        }

        #endregion
    }
}
