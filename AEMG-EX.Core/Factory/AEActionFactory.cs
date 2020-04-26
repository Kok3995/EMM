using Data;
using EMM.Core.Converter;
using EMM.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AEMG_EX.Core
{
    public class AEActionFactory : IAEActionFactory
    {
        public AEActionFactory(IPredefinedActionProvider actionProvider)
        {
            this.AEActionDict = new Dictionary<AEAction, Type>()
            {
                { AEAction.TrashMobBattle, typeof(TrashMobBattleViewModel) },
                { AEAction.EXPBattle, typeof(EXPBattleViewModel) },
                { AEAction.BossBattle, typeof(BossBattleViewModel) },
                { AEAction.FoodAD, typeof(FoodViewModel) },
                { AEAction.ReFoodAD, typeof(ReFoodViewModel) },
                { AEAction.Wait, typeof(WaitViewModel) },
            };

            var autoMapper = new SimpleAutoMapper();
            var turnFactory = new TurnFactory();

            this.DependencyDict = new Dictionary<Type, object>()
            {
                { typeof(SimpleAutoMapper), autoMapper },
                { typeof(ITurnFactory), turnFactory },
                { typeof(IPredefinedActionProvider),  actionProvider }
            };
        }

        Dictionary<AEAction, Type> AEActionDict;

        Dictionary<Type, object> DependencyDict;

        public IAEActionViewModel NewAEActionViewModel(AEAction aEAction)
        {
            if (!AEActionDict.ContainsKey(aEAction))
                throw new NotImplementedException("The AEAction is not implemented");

            Type instanceType = AEActionDict[aEAction];

            var constructorInfos = instanceType.GetConstructors();

            //TODO: Add a way to choose constructor
            var constructorInfo = constructorInfos[0];
            var parameters = constructorInfo.GetParameters();

            var parametersInstance = new List<object>();

            foreach (var param in parameters)
            {
                if (!DependencyDict.ContainsKey(param.ParameterType))
                    throw new NotImplementedException("The Dependency of "+ param.ParameterType + " " + "is not implemented");

                parametersInstance.Add(DependencyDict[param.ParameterType]);
            }

            return (IAEActionViewModel)constructorInfo.Invoke(parametersInstance.ToArray());
        }
    }
}
