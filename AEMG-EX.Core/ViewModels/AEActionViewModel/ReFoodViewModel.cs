using System.Collections.Generic;
using Data;
using EMM.Core;
using EMM.Core.Converter;

namespace AEMG_EX.Core
{
    public class ReFoodViewModel : FoodViewModel, IAEActionViewModel
    {
        #region Ctor

        public ReFoodViewModel(IPredefinedActionProvider actionProvider, SimpleAutoMapper autoMapper) : base(actionProvider, autoMapper)
        {
        }

        #endregion

        #region View Properties


        #endregion

        #region Interface implement

        public override AEAction AEAction => AEAction.ReFoodAD;

        #endregion

    }
}
