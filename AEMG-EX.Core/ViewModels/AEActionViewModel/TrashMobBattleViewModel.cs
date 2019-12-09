using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Data;
using EMM.Core.Converter;

namespace AEMG_EX.Core
{
    public class TrashMobBattleViewModel : BaseBattle, IAEActionViewModel
    {
        public TrashMobBattleViewModel(IPredefinedActionProvider actionProvider, SimpleAutoMapper autoMapper, ITurnFactory turnFactory) : base(actionProvider)
        {
            this.autoMapper = autoMapper;
            this.turnFactory = turnFactory;

            base.TurnList = new ObservableCollection<TurnViewModel>() { new TurnViewModel() };
            base.CurrentTurn = base.TurnList[0];

            base.InitializeCommand();
        }

        private SimpleAutoMapper autoMapper;
        private ITurnFactory turnFactory;

        public int ActionGroupIndex { get; set; }
        public int ActionIndex { get; set; }
        public virtual AEAction AEAction => AEAction.TrashMobBattle;
        public string ActionDescription { get; set; }

        public IAEAction ConvertBackToAction()
        {
            throw new System.NotImplementedException();
        }

        public IAEActionViewModel ConvertFromAction(IAEAction action)
        {
            throw new System.NotImplementedException();
        }

        public virtual void CopyOntoSelf(IAEActionViewModel source)
        {
            if (source.AEAction != AEAction || source.Equals(this))
                return;

            var sourceObject = source as TrashMobBattleViewModel;

            this.CopyTurnList(sourceObject.TurnList);
        }

        public virtual void CopyTurnList<TurnType>(ObservableCollection<TurnType> source) where TurnType : TurnViewModel, new()
        {
            var tempTurnList = new List<TurnType>();
            TurnType previous = null;

            foreach (var turn in source)
            {
                var newTurn = (TurnType)turnFactory.NewTurn(this.AEAction);
                autoMapper.SimpleAutoMap(turn, newTurn, ignoreTypes: new Type[] { typeof(TurnViewModel), typeof(ObservableCollection<Character>), typeof(Character), typeof(ICommand) });

                //copy character list
                var newBackline = turn.Backline.Select(c => new Character { CharacterIMG = c.CharacterIMG, CurrentAction = c.CurrentAction }).ToList();
                var newFrontline = turn.Frontline.Select(c => new Character { CharacterIMG = c.CharacterIMG, CurrentAction = c.CurrentAction, MoveIndex = c.MoveIndex, MoveCharacter = newBackline.Where(tc => tc.CharacterIMG == c.CharacterIMG).FirstOrDefault() });
                newTurn.Frontline = new ObservableCollection<Character>(newFrontline);
                newTurn.Backline = new ObservableCollection<Character>(newBackline);

                newTurn.PreviousTurn = previous;            
                previous = newTurn;
                tempTurnList.Add(newTurn);
            };

            //replace list
            this.TurnList = new ObservableCollection<TurnViewModel>(tempTurnList);
        }
    }
}
