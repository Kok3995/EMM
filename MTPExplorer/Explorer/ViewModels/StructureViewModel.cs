using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace MTPExplorer
{
    public class StructureViewModel: BaseViewModel
    {
        public StructureViewModel(IMTPManager manager, string initialDirectory = null)
        {
            this.manager = manager;
            this.initialDirectory = initialDirectory;

            Drives = GetDrives();

            InitializeCommands();
        }

        private string initialDirectory;
        private IMTPManager manager;

        public ObservableCollection<DirectoryItemsViewModel> Drives { get; set; }

        public DirectoryItemsViewModel SelectedItem { get; set; }

        public bool IsOKEnable => SelectedItem?.FullPath.Split(Path.DirectorySeparatorChar).Length > 2;

        public ICommand RefreshCommand { get; set; }
        public ICommand OKCommand { get; set; }

        internal EventHandler<MTPFolderSelectEventArgs> FolderSelected;

        internal void OpenInitialDirectory()
        {
            if (!manager.IsMTPPath(initialDirectory))
                return;

            var names = initialDirectory.Split(Path.DirectorySeparatorChar);

            var queue = new Queue<string>();

            for (int i = 1; i < names.Length; i++)
            {
                queue.Enqueue(names[i]);
            }

            ObservableCollection<DirectoryItemsViewModel> parent = Drives;

            while (queue.Count > 0)
            {
                var currentItem = queue.Dequeue();

                var nextItem = parent.Where(i => i.Name.Equals(currentItem)).FirstOrDefault();

                if (nextItem == null)
                    return;

                SelectedItem = nextItem;
                SelectedItem.IsSelected = true;

                nextItem.IsExpanded = true;

                parent = nextItem.ChildrenDirectory;
            }
        }

        private void InitializeCommands()
        {
            RefreshCommand = new RelayCommand(p =>
            {
                Drives = GetDrives();
            });

            OKCommand = new RelayCommand(p =>
            {
                FolderSelected?.Invoke(this, new MTPFolderSelectEventArgs(SelectedItem?.FullPath));
            }, p => IsOKEnable);
        }

        private ObservableCollection<DirectoryItemsViewModel> GetDrives()
        {
            //Create an empty list
            var children = this.manager.GetDevices();

            //Add logical drive to the list of item
            return new ObservableCollection<DirectoryItemsViewModel>(children.Select(child => new DirectoryItemsViewModel(child.Name, DirectoryItemsType.Drive, manager)));
        }
    }

    internal class MTPFolderSelectEventArgs : EventArgs
    {
        public MTPFolderSelectEventArgs(string path)
        {
            SelectedPath = path;
        }

        public string SelectedPath { get; set; }
    }
}
