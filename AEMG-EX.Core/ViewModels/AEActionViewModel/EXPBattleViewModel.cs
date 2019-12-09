using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Data;
using EMM;
using EMM.Core;
using EMM.Core.Converter;

namespace AEMG_EX.Core
{
    public class EXPBattleViewModel : TrashMobBattleViewModel, IAEActionViewModel
    {
        public EXPBattleViewModel(IPredefinedActionProvider actionProvider, SimpleAutoMapper autoMapper, ITurnFactory turnFactory) : base(actionProvider, autoMapper, turnFactory)
        {
            this.actionProvider = actionProvider;
            SelectedLeftRight = new ObservableCollection<Action>(new List<Action> { Action.RunLeft, Action.RunRight, Action.RunLeft, Action.RunRight });

            this.InitializeCommand();
        }

        private IPredefinedActionProvider actionProvider;

        #region View Properties

        /// <summary>
        /// Hold the left right order the user select
        /// </summary>
        public ObservableCollection<Action> SelectedLeftRight { get; set; }

        /// <summary>
        /// Number of battles to loop
        /// </summary>
        public int Loop { get; set; } = 1;

        #region Commands

        public ICommand AddLeftCommand { get; set; }
        public ICommand AddRightCommand { get; set; }
        public ICommand UndoSelectCommand { get; set; }

        protected override void InitializeCommand()
        {
            AddLeftCommand = new RelayCommand(p =>
            {
                this.SelectedLeftRight.Add(Action.RunLeft);
            });

            AddRightCommand = new RelayCommand(p =>
            {
                this.SelectedLeftRight.Add(Action.RunRight);
            });

            UndoSelectCommand = new RelayCommand(p =>
            {
                if (this.SelectedLeftRight == null && this.SelectedLeftRight.Count <= 0)
                    return;

                this.SelectedLeftRight.RemoveAt(this.SelectedLeftRight.Count - 1);
            }, p => this.SelectedLeftRight != null && this.SelectedLeftRight.Count > 0);
        }
        #endregion

        #endregion

        #region Interface Implement

        public override AEAction AEAction => AEAction.EXPBattle;

        public IAEActionViewModel Copy()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public override IList<IAction> UserChoicesToActionList()
        {
            var runList = new List<IAction>();

            //Add running left right to look for battle
            foreach (var select in SelectedLeftRight)
            {
                if (select == Action.RunLeft || select == Action.RunRight)
                    runList.AddRange(this.actionProvider.GetCharacterAction(select));
                else
                    throw new InvalidOperationException("Only support RunLeft, RunRight action");                        
            }

            //add battle
            runList.AddRange(base.UserChoicesToActionList());

            var completeList = new List<IAction>();

            for (int i = 0; i < Loop; i++)
            {
                completeList.AddRange(runList);
            }

            return completeList;
        }

        public override void CopyOntoSelf(IAEActionViewModel source)
        {
            base.CopyOntoSelf(source);

            var sourceObject = source as EXPBattleViewModel;

            this.Loop = sourceObject.Loop;

            SelectedLeftRight = new ObservableCollection<Action>(sourceObject.SelectedLeftRight);
        }
    }
}
