using Data;
using EMM;
using EMM.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AEMG_EX.Core
{
    public class TurnViewModel : BaseViewModel
    {
        #region Ctor

        public TurnViewModel()
        {
            InitializeCharacterList();

            InitializeCommand();
            this.FrontlineTempCount = this.Frontline.Count;
        }

        public TurnViewModel(TurnViewModel previous)
        {
            this.PreviousTurn = previous;
            //Move front back if character CurrentAction is Move or Reserve
            if (previous != null)
            {
                this.Frontline = new ObservableCollection<Character>(previous.Frontline.Select(c => new Character(c)));
                this.Backline = new ObservableCollection<Character>(previous.Backline.Select(c => new Character(c)));
                MoveCharacter();
            }
            else
                InitializeCharacterList();

            InitializeCommand();
            this.FrontlineTempCount = this.Frontline.Count;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Show turn order in the view
        /// </summary>
        public int TurnOrder => (PreviousTurn == null) ? 1 : PreviousTurn.TurnOrder + 1;

        public virtual BitmapImage ScreenIMG => (IsAF == false) ? AEMGStatic.BattleScreenNormal : AEMGStatic.AFScreen;

        /// <summary>
        /// The previous turn instance
        /// </summary>
        public TurnViewModel PreviousTurn { get; set; }

        /// <summary>
        /// List of frontline chars
        /// </summary>
        public ObservableCollection<Character> Frontline { get; set; }

        /// <summary>
        /// list of back line chars
        /// </summary>
        public ObservableCollection<Character> Backline { get; set; }

        /// <summary>
        /// The currently selected char
        /// </summary>
        public Character SelectedCharacter { get; set; }

        /// <summary>
        /// Selected char index in frontline
        /// </summary>
        public int SelectedFrontlineCharacterIndex { get; set; }

        /// <summary>
        /// Selected char index in backline
        /// </summary>
        public int SelectedBacklineCharacterIndex { get; set; }

        /// <summary>
        /// Indicate that Switch is selected. Remember to toggle this back
        /// </summary>
        public bool IsMove { get; set; }

        /// <summary>
        /// True if AF is clicked, toggle the second time
        /// </summary>
        public bool IsAF { get; set; }

        /// <summary>
        /// Show frontline char selected skill
        /// </summary>
        public bool IsSelectedSkillVisible { get; set; } = true;

        /// <summary>
        /// Show frontline skill select box
        /// </summary>
        public bool IsFrontlineSkillVisible { get; set; }

        /// <summary>
        /// Show backline skill select box
        /// </summary>
        public bool IsBacklineSkillVisible { get; set; }

        /// <summary>
        /// Show attack button
        /// </summary>
        public bool IsAttackButtonVisible => IsSelectedSkillVisible;

        /// <summary>
        /// Show reset button if the currently selected character action is frontline
        /// </summary>
        public bool IsResetFrontlineVisible => this.SelectedCharacter?.CurrentAction == CharacterAction.FrontLine;
        
        /// <summary>
        /// Temporary Count frontline number to disable frontline button appropriately
        /// </summary>
        public int FrontlineTempCount { get; set; }

        /// <summary>
        /// The number of character in frontline currently select reverse
        /// </summary>
        public int ReverseActionCount => this.Frontline.Where(c => c.CurrentAction == CharacterAction.Reserve).Count();

        /// <summary>
        /// The number of character in backline currently select frontline
        /// </summary>
        public int FrontlineActionCount => this.Backline.Where(c => c.CurrentAction == CharacterAction.FrontLine).Count();


        /// <summary>
        /// Show reserve button only if frontline member more than 1
        /// </summary>
        public bool IsReserveButtonVisible => (this.FrontlineTempCount - FrontlineActionCount) > 1 && !IsAttackButtonVisible && IsMove;

        /// <summary>
        /// How long should AF last. Default to 15 seconds
        /// </summary>
        public int AFTime { get; set; } = 15000;

        /// <summary>
        /// Time to wait for next turn. Default: 8s for normal battle, 12s for boss
        /// </summary>
        public virtual int WaitNextTurnTime { get; set; } = 8000;

        #endregion

        #region Commands

        public ICommand SelectCharacterCommand { get; set; }
        public ICommand SelectSkillCommand { get; set; }
        public ICommand SelectSwitchCommand { get; set; }
        public ICommand SelectFrontlineCommand { get; set; }
        public ICommand ResetFrontlineCommand { get; set; }       
        public ICommand SelectReserveCommand { get; set; }
        public ICommand ClearScreenCommand { get; set; }
        public ICommand SelectAFCommand { get; set; }
        public ICommand MoveCharacterRightCommand { get; set; }
        public ICommand MoveCharacterLeftCommand { get; set; }

        private void InitializeCommand()
        {
            SelectCharacterCommand = new RelayCommand(p => SelectCharacter((Character)p));
            SelectSkillCommand = new RelayCommand(p => SelectSkill((CharacterAction)(int.Parse(p.ToString()))));
            SelectSwitchCommand = new RelayCommand(p => SelectSwitch(), p => this.SelectedCharacter != null && !IsAF);
            SelectReserveCommand = new RelayCommand(p => SelectReserve());
            ClearScreenCommand = new RelayCommand(p => ClearScreen());
            SelectFrontlineCommand = new RelayCommand(p => SelectFrontline(), p => (this.FrontlineTempCount + ReverseActionCount) < 4 && SelectedCharacter?.CurrentAction != CharacterAction.Move);
            ResetFrontlineCommand = new RelayCommand(p => ResetFrontline(), p => this.SelectedCharacter?.CurrentAction == CharacterAction.FrontLine);
            SelectAFCommand = new RelayCommand(p => SelectAF());
            MoveCharacterRightCommand = new RelayCommand(p => MoveCharacterRight());
            MoveCharacterLeftCommand = new RelayCommand(p => MoveCharacterLeft());
        }


        #endregion

        #region Command Methods

        protected virtual void SelectCharacter(Character character)
        {
            HideAll();
            if (!IsMove)
            {
                if (this.Frontline.Contains(character))
                    this.IsFrontlineSkillVisible = true;
                else
                    this.IsBacklineSkillVisible = true;

                SelectedCharacter = character;
            }
            else
            {
                //Can not select frontline character to move
                if (this.Frontline.Contains(character))
                    return;

                //If move target is already select frontline then you can't swap with this character
                if (character.CurrentAction == CharacterAction.FrontLine)
                    return;

                character.CurrentAction = CharacterAction.DefaultSkill;
                var backlineIndex = this.Backline.IndexOf(character);
                this.SelectedCharacter.MoveIndex = backlineIndex + this.Frontline.Count;

                //check if the move character from last select is the same, if not set that character skill to defautl
                if (this.SelectedCharacter.MoveCharacter != null && !this.SelectedCharacter.MoveCharacter.CharacterIMG.Equals(character.CharacterIMG))
                {
                    this.SelectedCharacter.MoveCharacter.CurrentAction = CharacterAction.DefaultSkill;
                }

                CheckIfPreviousActionIsReverseThenIncreaseCount();

                this.SelectedCharacter.MoveCharacter = character;
                this.SelectedCharacter.CurrentAction = CharacterAction.Move;
                character.CurrentAction = CharacterAction.Move;
                this.IsMove = false;
                this.ClearScreen();
            }
        }

        protected virtual void SelectSkill(CharacterAction characterAction)
        {
            if (IsAF)
            {
                CharacterAction spam = CharacterAction.DefaultSkill;
                switch(characterAction)
                {
                    case CharacterAction.DefaultSkill:
                        spam = CharacterAction.AF0;
                        break;
                    case CharacterAction.SkillOne:
                        spam = CharacterAction.AF1;
                        break;
                    case CharacterAction.SkillTwo:
                        spam = CharacterAction.AF2;
                        break;
                    case CharacterAction.SkillThree:
                        spam = CharacterAction.AF3;
                        break;
                }

                this.SetSkillAllFrontline(spam);
                this.ClearScreen();
                return;
            }

            if (this.SelectedCharacter == null)
                return;

            HideAll();
            this.IsSelectedSkillVisible = true;

            CheckIfPreviousActionIsReverseThenIncreaseCount();

            //If previous action is move then reset the backline character action
            CheckIfPreviousActionIsMoveThenMoveCharacterActionToDefault();

            SelectedCharacter.CurrentAction = characterAction;
        }

        protected virtual void SelectSwitch()
        {
            if (SelectedCharacter == null)
                return;
            HideAll();
            this.IsMove = true;
        }

        protected virtual void SelectReserve()
        {
            if (this.FrontlineTempCount < 2 || IsMove == false)
                return;

            CheckIfPreviousActionIsMoveThenMoveCharacterActionToDefault();

            this.SelectedCharacter.CurrentAction = CharacterAction.Reserve;
            FrontlineTempCount -= 1;
            this.ClearScreen();
        }

        protected virtual void SelectFrontline()
        {
            if (SelectedCharacter == null || !this.Backline.Contains(this.SelectedCharacter) || this.Frontline.Count == 4)
                return;

            this.SelectedCharacter.CurrentAction = CharacterAction.FrontLine;
            this.FrontlineTempCount += 1;
            this.ClearScreen();
        }

        protected virtual void ResetFrontline()
        {
            if (!(this.SelectedCharacter?.CurrentAction == CharacterAction.FrontLine))
                return;

            this.SelectedCharacter.CurrentAction = CharacterAction.DefaultSkill;
            this.FrontlineTempCount -= 1;
            this.ClearScreen();
        }

        protected virtual void SelectAF()
        {
            this.IsAF ^= true;
            this.HideAll();
            if (IsAF)
            {
                this.IsFrontlineSkillVisible = true;
                SetSkillAllBackline(CharacterAction.DefaultSkill);
            }
            if (!IsAF)
            {
                SetSkillAllFrontline(CharacterAction.DefaultSkill);
                SetSkillAllBackline(CharacterAction.DefaultSkill);
                this.ClearScreen();
            }
        }

        protected virtual void ClearScreen()
        {
            this.HideAll();
            IsMove = false;
            this.IsSelectedSkillVisible = true;
            OnPropertyChanged(nameof(IsResetFrontlineVisible));
        }

        protected virtual void MoveCharacterRight()
        {
            if (SelectedCharacter == null)
                return;

            this.ClearScreen();

            var index = 0;

            if (this.Frontline.Contains(SelectedCharacter))
            {
                index = SelectedFrontlineCharacterIndex;

                //Not the last
                if (index < this.Frontline.Count - 1)
                {
                    SelectedCharacter.IsSelected = true;
                    this.Frontline.MoveSelectedElement(1);
                    SelectedCharacter.IsSelected = false;
                    return;
                }
                else //Last char in frontline, move to first char in back
                {
                    var newBackChar = new Character { CharacterIMG = SelectedCharacter.CharacterIMG };
                    this.Frontline.Remove(SelectedCharacter);
                    this.Backline.Insert(0, newBackChar);
                    //Set selected char
                    SelectedCharacter = newBackChar;
                    //minus 1 to temp frontline count
                    this.FrontlineTempCount -= 1;
                }
            }
            else
            {
                index = SelectedBacklineCharacterIndex;

                //last char in backline, do nothing
                if (index == this.Backline.Count - 1)
                    return;

                SelectedCharacter.IsSelected = true;
                this.Backline.MoveSelectedElement(1);
                SelectedCharacter.IsSelected = false;
            }
        }

        protected virtual void MoveCharacterLeft()
        {
            if (SelectedCharacter == null)
                return;

            var index = 0;

            if (this.Frontline.Contains(SelectedCharacter))
            {
                index = SelectedFrontlineCharacterIndex;

                //First char do nothing
                if (index < 1)
                    return;

                SelectedCharacter.IsSelected = true;
                this.Frontline.MoveSelectedElement(-1);
                SelectedCharacter.IsSelected = false;
            }
            else
            {
                index = SelectedBacklineCharacterIndex;

                //not first char in backline
                if (index > 0)
                {
                    SelectedCharacter.IsSelected = true;
                    this.Backline.MoveSelectedElement(-1);
                    SelectedCharacter.IsSelected = false;
                }
                else //first char in back line, move to the end of frontline
                {
                    var newfrontChar = new Character { CharacterIMG = SelectedCharacter.CharacterIMG };
                    this.Backline.Remove(SelectedCharacter);
                    this.Frontline.Add(newfrontChar);
                    SelectedCharacter = newfrontChar;
                    this.FrontlineTempCount += 1;
                }
            }
        }

        #endregion

        #region Helpers
        private void HideAll()
        {
            this.IsBacklineSkillVisible = false;
            this.IsFrontlineSkillVisible = false;
            this.IsSelectedSkillVisible = false;
        }

        private void SetSkillAllFrontline(CharacterAction skill)
        {
            foreach (var character in Frontline)
            {
                if (character.CurrentAction == CharacterAction.Reserve)
                    FrontlineTempCount += 1;

                character.CurrentAction = skill;
            }
        }

        private void SetSkillAllBackline(CharacterAction skill)
        {
            foreach (var character in Backline)
            {
                if (character.CurrentAction == CharacterAction.FrontLine)
                    FrontlineTempCount -= 1;
                character.CurrentAction = skill;
            }
        }

        private void MoveCharacter()
        {
            //Move character
            var charToMove = PreviousTurn.Frontline.Where(c => c.CurrentAction == CharacterAction.Move).ToList();
            if (charToMove.Count > 0)
            {
                foreach (var character in charToMove) {
                    var backCharIndex = character.MoveIndex - PreviousTurn.Frontline.Count;
                    var tempBackChar = this.Backline[backCharIndex];
                    var frontCharIndex = PreviousTurn.Frontline.IndexOf(character);
                    var tempFrontChar = this.Frontline[frontCharIndex];

                    this.Backline.Remove(tempBackChar);
                    this.Backline.Insert(backCharIndex, tempFrontChar); 
                    this.Frontline.Remove(tempFrontChar);
                    this.Frontline.Insert(frontCharIndex, tempBackChar);
                }
            }

            //Reserve character
            var charToReserve = PreviousTurn.Frontline.Where(c => c.CurrentAction == CharacterAction.Reserve);
            foreach (var character in charToReserve)
            {
                var tempCharToReserve = this.Frontline.Where(c => c.CharacterIMG == character.CharacterIMG).First();
                this.Frontline.Remove(tempCharToReserve);
                this.Backline.Add(tempCharToReserve);
            }

            //move character to front line
            var charToFrontline = PreviousTurn.Backline.Where(c => c.CurrentAction == CharacterAction.FrontLine);
            foreach (var character in charToFrontline)
            {
                var tempCharToFrontline = this.Backline.Where(c => c.CharacterIMG == character.CharacterIMG).First();
                this.Frontline.Add(tempCharToFrontline);
                this.Backline.Remove(tempCharToFrontline);
            }
        }

        private void CheckIfPreviousActionIsMoveThenMoveCharacterActionToDefault()
        {
            if (SelectedCharacter.CurrentAction == CharacterAction.Move)
                this.SelectedCharacter.MoveCharacter.CurrentAction = CharacterAction.DefaultSkill;
        }

        private void CheckIfPreviousActionIsReverseThenIncreaseCount()
        {
            if (SelectedCharacter.CurrentAction == CharacterAction.Reserve)
                this.FrontlineTempCount += 1;
        }

        private void InitializeCharacterList()
        {
            Frontline = new ObservableCollection<Character>()
            {
                new Character { CharacterIMG = AEMGStatic.C1 },
                new Character { CharacterIMG = AEMGStatic.C2 },
                new Character { CharacterIMG = AEMGStatic.C3 },
                new Character { CharacterIMG = AEMGStatic.C4 },
            };

            Backline = new ObservableCollection<Character>()
            {
                new Character { CharacterIMG = AEMGStatic.C5 },
                new Character { CharacterIMG = AEMGStatic.C6 },
            };
        }
        #endregion
    }
}
