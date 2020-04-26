using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Data;
using EMM.Core.Converter;
using PropertyChanged;

namespace EMM.Core.ViewModels
{
    public class CustomActionViewModel : ActionGroupViewModel
    {
        public CustomActionViewModel(SimpleAutoMapper autoMapper, ViewModelFactory viewModelFactory, ViewModelClipboard clipboard) : base(autoMapper, viewModelFactory, clipboard)
        {
            this.autoMapper = autoMapper;
            this.viewModelFactory = viewModelFactory;
        }

        private SimpleAutoMapper autoMapper;
        private ViewModelFactory viewModelFactory;

        public override BasicAction BasicAction { get => BasicAction.CustomAction; set { } }

        [DoNotSetChanged]
        public ObservableCollection<IActionViewModel> ActionsTreeView { get; set; }

        [DoNotSetChanged]
        public bool IsExpanded
        {
            get => ActionsTreeView?.Count > 0;
            set
            {
                if (value)
                    ActionsTreeView = new ObservableCollection<IActionViewModel>(ViewModelList);
                else
                    ActionsTreeView?.Clear();
            }
        }

        [DoNotSetChanged]
        public override IActionViewModel SelectedItem { get => base.SelectedItem; set => base.SelectedItem = value; }
    }
}
