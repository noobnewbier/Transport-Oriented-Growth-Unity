using System;
using TransportOrientedGrowthTree.Ui;

namespace TransportOrientedGrowthTree.Di.Injectors
{
    public class Injector
    {
        private readonly IDependenciesProvider _dependenciesProvider;

        public Injector(IDependenciesProvider dependenciesProvider)
        {
            _dependenciesProvider = dependenciesProvider;
        }

        public void InjectToInjectable(IInjectable injectable)
        {
            var injectMethod = ReflectionUtils.GetMethodByAttribute(injectable.GetType(), typeof(InjectAttribute));
            var parameterInfos = injectMethod.GetParameters();
            var parameters = new object[parameterInfos.Length];

            for (var i = 0; i < parameterInfos.Length; i++)
            {
                var paramType = parameterInfos[i].ParameterType;

                if (_dependenciesProvider.TryGet<object>(paramType, out var param))
                    parameters[i] = param;
                else
                    throw new InvalidOperationException($"{paramType.FullName} is not included in the dependencies");
            }


            injectMethod.Invoke(injectable, parameters);
        }
    }
}