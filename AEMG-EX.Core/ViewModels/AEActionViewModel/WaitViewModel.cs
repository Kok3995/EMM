using System.Collections.Generic;
using Data;
using EMM.Core;
using EMM.Core.Converter;

namespace AEMG_EX.Core
{
    public class WaitViewModel : BaseViewModel, IAEActionViewModel
    {
        #region Ctor

        public WaitViewModel(IPredefinedActionProvider actionProvider, SimpleAutoMapper autoMapper)
        {
            this.actionProvider = actionProvider;
            this.autoMapper = autoMapper;
        }

        private IPredefinedActionProvider actionProvider;
        private SimpleAutoMapper autoMapper;


        #endregion

        #region View Properties

        /// <summary>
        /// Time to wait in milisecond
        /// </summary>
        public int WaitTime { get; set; }

        #endregion

        #region Interface implement

        public AEAction AEAction => AEAction.Wait;

        public int ActionGroupIndex { get; set; }
        public int ActionIndex { get; set; }
        public string ActionDescription { get; set; }

        public IAEAction ConvertBackToAction()
        {
            return autoMapper.SimpleAutoMap<WaitViewModel, AEWait>(this);
        }

        public IAEActionViewModel ConvertFromAction(IAEAction action)
        {
            if (action.AEAction != AEAction)
                return null;

            if (!(action is AEWait wait))
                return null;

            autoMapper.SimpleAutoMap(wait, this);

            return this;
        }

        public IList<IAction> UserChoicesToActionList()
        {
            return new List<IAction> { this.actionProvider.GetWait(this.WaitTime) };
        }

        public void CopyOntoSelf(IAEActionViewModel source)
        {
            if (source.AEAction != AEAction || source.Equals(this))
                return;

            var sourceObject = source as WaitViewModel;

            this.WaitTime = sourceObject.WaitTime;
        }

        #endregion

    }
}
