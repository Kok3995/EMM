using Data;
using System;
using System.Collections.Generic;

namespace AEMG_EX.Core
{
    public class TurnFactory : ITurnFactory
    {
        private Dictionary<AEAction, Type> turnDict = new Dictionary<AEAction, Type>()
        {
            { AEAction.TrashMobBattle,  typeof(TurnViewModel) },
            { AEAction.BossBattle,  typeof(BossTurnViewModel) },
            { AEAction.EXPBattle,  typeof(TurnViewModel) },
        };

        public TurnViewModel NewTurn(AEAction aEAction)
        {
            if (!turnDict.ContainsKey(aEAction))
                throw new NotImplementedException("The turn for this action has not yet implement");

            return (TurnViewModel)Activator.CreateInstance(turnDict[aEAction]);
        }
    }
}
