using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace MTPExplorer
{
    public class DirectoryItemsViewModel: BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default contructor: Create a new directory item
        /// </summary>
        public DirectoryItemsViewModel(string name, DirectoryItemsType type, IMTPManager manager, DirectoryItemsViewModel parent = null)
        {
            Name = name;
            Type = type;
            Parent = parent;
            this.manager = manager;

            ExpandCommand = new RelayCommand(p => ExpandDirectory());

            ClearChildren();
        }

        #endregion

        private IMTPManager manager;

        #region Public Properties

        public DirectoryItemsViewModel Parent { get; set; }

        /// <summary>
        /// Type of directory: file, folder, drive
        /// </summary>
        public DirectoryItemsType Type { get; set; }

        /// <summary>
        /// Name of the directory
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The full path of the directory
        /// </summary>
        public string FullPath => Type == DirectoryItemsType.Drive ? string.Format("mtp:{0}{1}", Path.DirectorySeparatorChar, Name) : Parent.FullPath + Path.DirectorySeparatorChar + Name;

        public ObservableCollection<DirectoryItemsViewModel> ChildrenDirectory { get; set; }

        /// <summary>
        /// Only drives and folders can have a expand button
        /// </summary>
        public bool CanExpand
        {
            get
            {
                return (this.Type != DirectoryItemsType.File);
            }
        }

        /// <summary>
        /// Check whether if the directory has expanded or not
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return (this.ChildrenDirectory?.Count(f => f != null) > 0);
            }
            set
            {         
                //If the UI tells to expand
                if (value == true)
                    ExpandDirectory();
                else
                    ClearChildren();
            }
        }

        /// <summary>
        /// When item is selected
        /// </summary>
        public bool IsSelected { get; set; }

        #endregion

        #region public commands

        public ICommand ExpandCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Methods to expand a directory
        /// </summary>
        public void ExpandDirectory()
        {
            //Get the children of the directory
            var children = manager.GetContentsFromPath(FullPath).Where(c => c.IsFolder);

            //Cast and add them to the list
            this.ChildrenDirectory = new ObservableCollection<DirectoryItemsViewModel>(children.Select(f => new DirectoryItemsViewModel(f.Name, f.IsFolder ? DirectoryItemsType.Folder : DirectoryItemsType.File, manager, this)));
        }

        /// <summary>
        /// Methods to clear children when directory collapsed
        /// </summary>
        public void ClearChildren()
        {
            //Clear directory list
            this.ChildrenDirectory = new ObservableCollection<DirectoryItemsViewModel>();

            //Add null item to show expand icon if it's not a file
            if (this.CanExpand == true)
                ChildrenDirectory.Add(null);
        }

        #endregion
    }
}


