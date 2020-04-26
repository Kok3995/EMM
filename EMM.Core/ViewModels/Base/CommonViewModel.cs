using PropertyChanged;
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
            InitializeCommandsAndEvent();
        }

        public CommonViewModel(ViewModelClipboard clipboard)
        {
            this.clipboard = clipboard;
            InitializeCommandsAndEvent();
        }

        #region Private members

        /// <summary>
        /// Use to undo cut command, save a new backup list to set back
        /// </summary>
        private ObservableCollection<IActionViewModel> backupCutList;

        private ViewModelClipboard clipboard;

        #endregion

        #region Public Properties

        /// <summary>
        /// List of action group
        /// </summary>
        public ObservableCollection<IActionViewModel> ViewModelList { get; set; } = new ObservableCollection<IActionViewModel>();

        public void OnViewModelListChanged()
        {
            ViewModelList.CollectionChanged += (s, e) =>
            {
                IsChanged = true;
            };
        }

        [DoNotSetChanged]
        /// <summary>
        /// The selected item
        /// </summary>
        public virtual IActionViewModel SelectedItem { get; set; }

        [DoNotSetChanged]
        /// <summary>
        /// The selected items
        /// </summary>
        public ObservableCollection<IActionViewModel> SelectedItems { get; set; }

        [DoNotSetChanged]
        /// <summary>
        /// The index of the selected group, default to -1 so insert work first time
        /// </summary>
        public virtual int SelectedItemIndex { get; set; }/* = -1;*/

        [DoNotSetChanged]
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
        public ICommand ToggleActionCommand { get; set; }

        /// <summary>
        /// Initilize all commands in this viewmodel
        /// </summary>
        private void InitializeCommandsAndEvent()
        {
            AddCommand = new RelayCommand(AddItem);
            InsertCommand = new RelayCommand(InsertItem);
            CopyCommand = new RelayCommand(CopyItem, p => SelectedItem != null && SelectedItemIndex > -1);
            CutCommand = new RelayCommand(CutItem, p => SelectedItem != null && SelectedItemIndex > -1);
            UndoCutCommand = new RelayCommand(UndoCutItem);
            PasteCommand = new RelayCommand(PasteItem, p => clipboard.GetCopiedCount() > 0);
            DeleteCommand = new RelayCommand(DeleteItem, p => this.SelectedItem != null && SelectedItemIndex > -1);
            UpCommand = new RelayCommand(MoveItemUp, p => SelectedItem != null && SelectedItemIndex > -1);
            DownCommand = new RelayCommand(MoveItemDown, p => SelectedItem != null && SelectedItemIndex > -1);
            ToggleActionCommand = new RelayCommand(ToggleAction, p => SelectedItem != null && SelectedItemIndex > -1);          
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

            clipboard.Copy(selectedItems, this.ViewModelList);
        }

        protected virtual void CutItem(object parameter)
        {
            var selectedItems = this.ViewModelList.GetSelectedElement();

            clipboard.Cut(selectedItems, this.ViewModelList);

            this.backupCutList = new ObservableCollection<IActionViewModel>(this.ViewModelList);
        }
        protected virtual void UndoCutItem(object parameter)
        {
            throw new NotImplementedException();
        }
        protected virtual void PasteItem(object parameter)
        {           
            int pasteIndex = this.ViewModelList.GetPasteIndex(SelectedItemIndex);
           
            for (int i = 1; i <= clipboard.GetCopiedCount(); i++)
            {
                var newItem = clipboard.Get(i - 1).MakeCopy();

                this.ViewModelList.Insert(pasteIndex + i, newItem);

                newItem.IsSelected = true;
            }

            clipboard.RemoveCopiedItemIfCut();
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
            var temp = ViewModelList.GetSelectedElement();

            this.ViewModelList.MoveSelectedElement(-1);

            temp.ForEach(v => v.IsSelected = true);
        }
        protected virtual void MoveItemDown(object parameter)
        {
            var temp = ViewModelList.GetSelectedElement();

            this.ViewModelList.MoveSelectedElement(1);

            temp.ForEach(v => v.IsSelected = true);
        }
        protected virtual void ToggleAction(object parameter)
        {
            var selectedItems = this.ViewModelList.GetSelectedElement();
            foreach (var item in selectedItems)
            {
                item.IsDisable ^= true;
            }
        }

        #endregion
    }

    /// <summary>
    /// Clipboard for all viewmodel
    /// </summary>
    public class ViewModelClipboard
    {
        /// <summary>
        /// List of item to be copied <see cref="IActionViewModel"/>
        /// </summary>
        private List<IActionViewModel> copiedList;

        /// <summary>
        /// List of action group
        /// </summary>
        private ObservableCollection<IActionViewModel> ViewModelList;

        /// <summary>
        /// Indicate if cut command is used previously
        /// </summary>
        public bool IsCut { get; set; }

        public int GetCopiedCount()
        {
            return copiedList?.Count ?? 0;
        }

        public void Clear()
        {
            copiedList?.Clear();
        }

        public void Copy(List<IActionViewModel> copiedList, ObservableCollection<IActionViewModel> mainList)
        {
            this.copiedList = copiedList;
            ViewModelList = mainList;
            IsCut = false;
        }

        public void Cut(List<IActionViewModel> copiedList, ObservableCollection<IActionViewModel> mainList)
        {
            Copy(copiedList, mainList);
            IsCut = true;
        }

        public IActionViewModel Get(int index)
        {
            return copiedList[index];
        }

        public void RemoveCopiedItemIfCut()
        {
            if (this.IsCut == true)
            {
                foreach (var group in this.copiedList)
                    this.ViewModelList.Remove(group);

                Clear();
                IsCut = false;
            }
        }
    }
}
