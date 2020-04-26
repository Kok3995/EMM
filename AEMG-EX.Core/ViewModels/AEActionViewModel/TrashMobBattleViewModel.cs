using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        public virtual IAEAction ConvertBackToAction()
        {
            var trash = autoMapper.SimpleAutoMap<TrashMobBattleViewModel, Battle>(this);

            trash.TurnList = this.TurnList.Select(t =>
            {
                var turn = new Turn
                {
                    AFTime = t.AFTime,
                    FrontlineTempCount = t.FrontlineTempCount,
                    IsAF = t.IsAF,
                    WaitNextTurnTime = t.WaitNextTurnTime,
                    TurnType = this.AEAction,
                    Frontline = t.Frontline.Select(c => new Character { CurrentAction = c.CurrentAction, MoveIndex = c.MoveIndex, CharacterIMG = c.CharacterIMG, MoveCharacterPosition = c.MoveCharacter?.CharacterIMG }).ToList(),
                    Backline = t.Backline.Select(c => new Character { CurrentAction = c.CurrentAction, MoveIndex = c.MoveIndex, CharacterIMG = c.CharacterIMG, MoveCharacterPosition = c.MoveCharacter?.CharacterIMG }).ToList(),
                };

                return turn;
            }).ToList();

            return trash;
        }

        public virtual IAEActionViewModel ConvertFromAction(IAEAction action)
        {
            if (action.AEAction != AEAction)
                return null;

            if (!(action is Battle battle))
                return null;

            IList<TurnViewModel> turnList = new List<TurnViewModel>();

            foreach (var turn in battle.TurnList)
            {
                var turnVM = turnFactory.NewTurn(turn.TurnType);
                autoMapper.SimpleAutoMap<Turn, TurnViewModel>(turn, turnVM);

                //Convert backline first
                turnVM.Backline = new ObservableCollection<CharacterViewModel>(turn.Backline.Select(c =>
                {
                    var character = autoMapper.SimpleAutoMap<Character, CharacterViewModel>(c);

                    return character;
                }));

                turnVM.Frontline = new ObservableCollection<CharacterViewModel>(turn.Frontline.Select(c =>
                {
                    var character = autoMapper.SimpleAutoMap<Character, CharacterViewModel>(c);

                    character.MoveCharacter = (c.MoveCharacterPosition == null) ? null : turnVM.Backline.Where(bc => bc.CharacterIMG == c.MoveCharacterPosition).FirstOrDefault();

                    return character;
                }));


                turnList.Add(turnVM);
            }

            CopyTurnList(turnList);
            
            return this;
        }

        public virtual void CopyOntoSelf(IAEActionViewModel source)
        {
            if (source.AEAction != AEAction || source.Equals(this))
                return;

            var sourceObject = source as TrashMobBattleViewModel;

            this.CopyTurnList(sourceObject.TurnList);
        }

        public virtual void CopyTurnList<TurnType>(IList<TurnType> source) where TurnType : TurnViewModel, new()
        {
            var tempTurnList = new List<TurnType>();
            TurnType previous = null;

            foreach (var turn in source)
            {
                var newTurn = (TurnType)turnFactory.NewTurn(this.AEAction);
                autoMapper.SimpleAutoMap(turn, newTurn, ignoreTypes: new Type[] { typeof(TurnViewModel), typeof(ObservableCollection<CharacterViewModel>), typeof(CharacterViewModel), typeof(ICommand) });

                //copy character list
                var newBackline = turn.Backline.Select(c => new CharacterViewModel { CharacterIMG = c.CharacterIMG, CurrentAction = c.CurrentAction }).ToList();
                var newFrontline = turn.Frontline.Select(c => new CharacterViewModel { CharacterIMG = c.CharacterIMG, CurrentAction = c.CurrentAction, MoveIndex = c.MoveIndex, MoveCharacter = newBackline.Where(tc => tc.CharacterIMG == c.MoveCharacter?.CharacterIMG).FirstOrDefault() });
                newTurn.Frontline = new ObservableCollection<CharacterViewModel>(newFrontline);
                newTurn.Backline = new ObservableCollection<CharacterViewModel>(newBackline);

                newTurn.PreviousTurn = previous;            
                previous = newTurn;
                tempTurnList.Add(newTurn);
            };

            //replace list
            this.TurnList = new ObservableCollection<TurnViewModel>(tempTurnList);
            if (this.TurnList.Count > 0)
                this.CurrentTurn = TurnList[0];
        }
    }
}
