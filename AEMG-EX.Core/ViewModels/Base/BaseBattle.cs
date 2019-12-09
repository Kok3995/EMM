using Data;
using EMM;
using EMM.Core;
using EMM.Core.Converter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AEMG_EX.Core
{
    /// <summary>
    /// Base class for all battles
    /// </summary>
    public class BaseBattle : BaseViewModel
    {
        public BaseBattle(IPredefinedActionProvider actionProvider)
        {
            this.actionProvider = actionProvider;
        }

        private IPredefinedActionProvider actionProvider;

        public virtual ObservableCollection<TurnViewModel> TurnList { set; get; }

        public virtual TurnViewModel CurrentTurn { get; set; }

        public int CurrentTurnIndex { get; set; }

        public int TotalTurn => TurnList.Count;

        #region Commands

        public ICommand AddTurnCommand { get; set; }
        public ICommand RemoveTurnCommand { get; set; }

        protected virtual void InitializeCommand()
        {
            AddTurnCommand = new RelayCommand(p => AddTurn());
            RemoveTurnCommand = new RelayCommand(p => RemoveTurn(), p => TotalTurn > 1);
        }

        #endregion

        #region Command Methods

        protected virtual void AddTurn()
        {
            if (TotalTurn == 0) {
                this.TurnList.Add(new TurnViewModel());
            }

            this.TurnList.Add(new TurnViewModel(this.TurnList[TotalTurn - 1]));
            CurrentTurn = this.TurnList[TotalTurn - 1];
        }

        protected virtual void RemoveTurn()
        {
            if (TotalTurn == 1)
                return;

            this.TurnList.RemoveAt(TotalTurn - 1);
        }

        #endregion

        public virtual IList<IAction> UserChoicesToActionList()
        {
            var list = new List<IAction>();
            
            foreach(var turn in TurnList)
            {
                List<IAction> temp = new List<IAction>();
                
                //Check AF
                if (turn.IsAF)
                {
                    temp.AddRange(this.actionProvider.GetCharacterAction(Action.ClickAF));
                    temp.AddRange(this.actionProvider.GetAFSpamClick(turn.Frontline[0].CurrentAction, turn.AFTime));
                }
                else
                {
                    //frontline
                    foreach (var character in turn.Frontline)
                    {
                        var frontlineIndex = turn.Frontline.IndexOf(character);
                        //move
                        if (character.CurrentAction == CharacterAction.Move)
                        {
                            temp.AddRange(this.actionProvider.GetCharacterAction(this.CharacterIndexToClickLocation(frontlineIndex)));
                            temp.AddRange(this.actionProvider.GetCharacterAction(Action.ClickSwitch_FrontLine));
                            temp.AddRange(this.actionProvider.GetCharacterAction(this.CharacterIndexToClickLocation(character.MoveIndex)));
                            continue;
                        }

                        //reserve
                        if (character.CurrentAction == CharacterAction.Reserve)
                        {
                            temp.AddRange(this.actionProvider.GetCharacterAction(this.CharacterIndexToClickLocation(frontlineIndex)));
                            temp.AddRange(this.actionProvider.GetCharacterAction(Action.ClickSwitch_FrontLine));
                            temp.AddRange(this.actionProvider.GetCharacterAction(Action.ClickSub));
                            continue;
                        }

                        var charFromLastTurn = turn.PreviousTurn?.Frontline.Where(c => c.CharacterIMG == character.CharacterIMG).FirstOrDefault();

                        //if from backline
                        if (charFromLastTurn == null)
                        {
                            //Check if not changing from Default skill
                            if (character.CurrentAction == CharacterAction.DefaultSkill)
                                continue;
                            else //change from default to something else
                            {
                                temp.AddRange(this.actionProvider.GetCharacterAction(this.CharacterIndexToClickLocation(frontlineIndex)));
                                temp.AddRange(this.actionProvider.GetSkillClick(character.CurrentAction));
                            }
                        }
                        else //didnt move, reserve, or from backline
                        {
                            //change skill if different from previous action
                            if (character.CurrentAction != charFromLastTurn.CurrentAction)
                            {
                                temp.AddRange(this.actionProvider.GetCharacterAction(this.CharacterIndexToClickLocation(frontlineIndex)));
                                temp.AddRange(this.actionProvider.GetSkillClick(character.CurrentAction));
                            }
                        }
                    }

                    //backline
                    foreach (var character in turn.Backline)
                    {
                        if (character.CurrentAction != CharacterAction.FrontLine)
                            continue;
                        var backlineIndex = turn.Frontline.Count + turn.Backline.IndexOf(character);
                        temp.AddRange(this.actionProvider.GetCharacterAction(this.CharacterIndexToClickLocation(backlineIndex)));
                        temp.AddRange(this.actionProvider.GetCharacterAction(Action.ClickSwitch_FrontLine));
                    }
                }

                temp.Add(this.actionProvider.GetWait(turn.WaitNextTurnTime));
                list.AddRange(temp);
            }

            return list;
        }

        private Action CharacterIndexToClickLocation(int index)
        {
            switch (index)
            {
                case 0:
                    return Action.ClickFirstCharacter;
                case 1:
                    return Action.ClickSecondCharacter;
                case 2:
                    return Action.ClickThirdCharacter;
                case 3:
                    return Action.ClickFourthCharacter;
                case 4:
                    return Action.ClickFifthCharacter;
                case 5:
                    return Action.ClickSixthCharacter;
                default:
                    return Action.ClickFirstCharacter;
            }
        }

    }
}
