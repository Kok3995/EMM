using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data;
using EMM.Core;
using EMM.Core.Converter;

namespace AEMG_EX.Core
{
    public class BossBattleViewModel : TrashMobBattleViewModel, IAEActionViewModel
    {
        public BossBattleViewModel(IPredefinedActionProvider actionProvider, SimpleAutoMapper autoMapper, ITurnFactory turnFactory) : base(actionProvider, autoMapper, turnFactory)
        {
            base.TurnList = new ObservableCollection<TurnViewModel>() { new BossTurnViewModel() };
            base.CurrentTurn = base.TurnList[0];

            base.InitializeCommand();
        }


        public override AEAction AEAction => AEAction.BossBattle;

        protected override void AddTurn()
        {
            if (TotalTurn == 0)
            {
                this.TurnList.Add(new BossTurnViewModel());
            }

            this.TurnList.Add(new BossTurnViewModel(this.TurnList[TotalTurn - 1] as BossTurnViewModel));
            CurrentTurn = this.TurnList[TotalTurn - 1];
        }
    }
}
