using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Data;
using EMM;
using EMM.Core;
using EMM.Core.Converter;

namespace AEMG_EX.Core
{
    public class FoodViewModel : BaseViewModel, IAEActionViewModel
    {
        #region Ctor

        public FoodViewModel(IPredefinedActionProvider actionProvider, SimpleAutoMapper autoMapper)
        {
            this.actionProvider = actionProvider;
            this.autoMapper = autoMapper;

            InitializeCommands();
        }

        private IPredefinedActionProvider actionProvider;
        private SimpleAutoMapper autoMapper;

        #endregion

        #region View Properties

        public string ActionDescription { get; set; }

        public bool EatOrNotToEat { get; set; }

        #endregion

        public ICommand BackCommand { get; set; }

        public ICommand UseCommand { get; set; }

        private void InitializeCommands()
        {
            BackCommand = new RelayCommand(p =>
            {
                this.EatOrNotToEat = false;
            });

            UseCommand = new RelayCommand(p =>
            {
                this.EatOrNotToEat = true;
            });
        }


        #region Interface implement

        public int ActionGroupIndex { get; set; }

        public int ActionIndex { get; set; }

        public virtual AEAction AEAction => AEAction.FoodAD;

        public virtual IAEAction ConvertBackToAction()
        {
            return autoMapper.SimpleAutoMap<FoodViewModel, Food>(this);
        }

        public virtual IAEActionViewModel ConvertFromAction(IAEAction action)
        {
            if (action.AEAction != AEAction)
                return null;

            if (!(action is Food food))
                return null;

            autoMapper.SimpleAutoMap(food, this);

            return this;
        }

        public IAEActionViewModel Copy()
        {
            throw new System.NotImplementedException();
        }

        public virtual IList<IAction> UserChoicesToActionList()
        {
            if (!EatOrNotToEat)
                return new List<IAction>();

            return this.actionProvider.GetCharacterAction((AEAction == AEAction.FoodAD) ? Action.FoodAD : Action.ReFoodAD);
        }

        public void CopyOntoSelf(IAEActionViewModel source)
        {
            if (source.AEAction != AEAction)
                return;

            this.EatOrNotToEat = (source as FoodViewModel).EatOrNotToEat;
        }

        #endregion

    }
}
