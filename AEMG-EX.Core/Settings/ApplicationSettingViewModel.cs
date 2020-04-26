using Data;
using EMM;
using EMM.Core;
using EMM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AEMG_EX.Core
{
    public class ApplicationSettingViewModel : BaseViewModel
    {
        public ApplicationSettingViewModel(IPredefinedActionProvider actionProvider, ViewModelFactory viewModelFactory)
        {
            this.actionProvider = actionProvider;
            this.viewModelFactory = viewModelFactory;
            defaultActionViewModelDict = new Dictionary<Action, List<IActionViewModel>>();

            InitializeCommands();
        }

        private IPredefinedActionProvider actionProvider;
        private ViewModelFactory viewModelFactory;
        private Dictionary<Action, List<IActionViewModel>> defaultActionViewModelDict;

        public int X { get; set; }

        public int Y { get; set; }

        public ObservableCollection<IActionViewModel> Actions { get; set; }

        public bool IsKeepAfterUpdate { get; set; }

        public ICommand SaveApplicationSettingCommand { get; set; }

        /// <summary>
        /// Fire when ok button is click and setting is saved
        /// </summary>
        public event EventHandler SettingSaved;

        private void InitializeCommands()
        {
            SaveApplicationSettingCommand = new RelayCommand(p =>
            {
                var defaultAction = actionProvider.GetDefaultActions();

                defaultAction.X = X;
                defaultAction.Y = Y;
                defaultAction.IsKeepAfterUpdate = IsKeepAfterUpdate;

                defaultAction.Dict = defaultActionViewModelDict.ToDictionary(k => k.Key, v => v.Value.Select(vm => vm.ConvertBackToAction()).ToList());


                if (actionProvider.SaveDefaultActions(defaultAction));
                    SettingSaved?.Invoke(this, EventArgs.Empty);
            });
        }

        public void LoadSetting()
        {         
            var defaultAction = actionProvider.GetDefaultActions();

            X = defaultAction.X;
            Y = defaultAction.Y;
            IsKeepAfterUpdate = defaultAction.IsKeepAfterUpdate;

            if (Actions == null)
                Actions = new ObservableCollection<IActionViewModel>();

            Actions.Clear();
            defaultActionViewModelDict.Clear();

            foreach (var item in defaultAction.Dict)
            {
                foreach (var action in item.Value)
                {
                    var vm = viewModelFactory.NewActionViewModel(action.BasicAction).ConvertFromAction(action);
                    Actions.Add(vm);

                    if (!defaultActionViewModelDict.ContainsKey(item.Key))
                    {
                        defaultActionViewModelDict.Add(item.Key, new List<IActionViewModel>());
                    }

                    defaultActionViewModelDict[item.Key].Add(vm);
                }
            }
        }
    }
}
