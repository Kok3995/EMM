using Data;
using EMM.Core.Converter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EMM.Core.ViewModels
{
    public class ActionGroupViewModel : CommonViewModel, IActionViewModel
    {
        public ActionGroupViewModel(SimpleAutoMapper autoMapper, ViewModelFactory viewModelFactory)
        {
            this.autoMapper = autoMapper;
            this.viewModelFactory = viewModelFactory;

            InitializeCommands();
        }

        #region Private members

        private SimpleAutoMapper autoMapper;
        private ViewModelFactory viewModelFactory;

        #endregion

        #region Public Properties
        public BasicAction BasicAction { get => BasicAction.ActionGroup; set { } }

        /// <summary>
        /// Description of the action group
        /// </summary>
        public string ActionDescription { get; set; } = "New Group";

        /// <summary>
        /// Repeat this action group
        /// </summary>
        public int Repeat { get; set; } = 1;

        #endregion

        #region Commands   

        /// <summary>
        /// Initilize all commands in this viewmodel
        /// </summary>
        private void InitializeCommands()
        {
            
        }

        #endregion

        #region Interface Implement

        public IAction ConvertBackToAction()
        {
            var actionGroup = this.autoMapper.SimpleAutoMap<ActionGroupViewModel, ActionGroup>(this);
            actionGroup.ActionList = new List<IAction>();

            foreach (var actionViewModel in this.ViewModelList)
            {
                actionGroup.ActionList.Add(actionViewModel.ConvertBackToAction());
            }

            return actionGroup;
        }
        public IActionViewModel MakeCopy()
        {
            var newGroup = (ActionGroupViewModel)viewModelFactory.NewActionViewModel(this.BasicAction);
            this.autoMapper.SimpleAutoMap<ActionGroupViewModel, ActionGroupViewModel>(this, newGroup, new List<Type> { typeof(ICommand), typeof(ObservableCollection<IActionViewModel>) });
            newGroup.ViewModelList = new ObservableCollection<IActionViewModel>(this.ViewModelList.Select(i => i.MakeCopy()));
            return newGroup;
        }
        public IActionViewModel ConvertFromAction(IAction action)
        {
            var actionGroup = action as ActionGroup;
            this.autoMapper.SimpleAutoMap<ActionGroup, ActionGroupViewModel>(actionGroup as ActionGroup, this);
            ViewModelList = this.LoadActionsViewModel(actionGroup.ActionList);
            return this;
        }

        #endregion

        #region Override Methods

        protected override void AddItem(object parameter)
        {
            try
            {
                base.ViewModelList.Add(base.SelectedItem = viewModelFactory.NewActionViewModel(ActionBaseOnCommandParameter(parameter)));
            }
            catch { }
        }

        protected override void InsertItem(object parameter)
        {
            try
            {
                base.ViewModelList.Insert(SelectedItemIndex + 1, SelectedItem = viewModelFactory.NewActionViewModel(ActionBaseOnCommandParameter(parameter)));
            }
            catch { }
        } 

        #endregion

        #region Helpers

        /// <summary>
        /// Load list of <see cref="IActionViewModel"/> from a list of <see cref="IAction"/>
        /// </summary>
        /// <param name="actionList">the list of <see cref="IAction"/> to load/></param>
        protected ObservableCollection<IActionViewModel> LoadActionsViewModel(IList<IAction> actionList)
        {
            var list = new ObservableCollection<IActionViewModel>();

            foreach (var action in actionList)
            {
                var viewmodel = this.viewModelFactory.NewActionViewModel(action.BasicAction).ConvertFromAction(action);
                if (viewmodel == null)
                    continue;
                list.Add(viewmodel);
            }

            return list;
        }

        protected BasicAction ActionBaseOnCommandParameter(object parameter)
        {
            BasicAction action = default;
            switch ((string)parameter)
            {
                case "Click":
                    action = BasicAction.Click;
                    break;
                case "Swipe":
                    action = BasicAction.Swipe;
                    break;
                case "Wait":
                    action = BasicAction.Wait;
                    break;
                case "Recorded":
                    action = BasicAction.Recorded;
                    break;
                case "AE":
                    action = BasicAction.AE;
                    break;
            }

            return action;
        }

        #endregion
    }
}
