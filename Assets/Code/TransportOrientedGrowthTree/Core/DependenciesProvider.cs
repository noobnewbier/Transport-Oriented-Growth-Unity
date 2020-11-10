using System;
using System.Collections.Generic;

namespace TransportOrientedGrowthTree.Core
{
    public interface IDependenciesProvider
    {
        bool TryGet(Type type, out object t);
        void AddT<T>(T t);
    }

    public class DependenciesProvider : IDependenciesProvider
    {
        private readonly IDictionary<Type, object> _dependencies;

        public DependenciesProvider()
        {
            _dependencies = new Dictionary<Type, object>();
        }

        public bool TryGet(Type type, out object t)
        {
            if (_dependencies.ContainsKey(type))
            {
                t = _dependencies[type];
                return true;
            }
            
            t = default;
            return false;
        }

        public void AddT<T>(T t)
        {
            if (_dependencies.ContainsKey(typeof(T)))
            {
                throw new ArgumentException($"{typeof(T).FullName} already has its dependencies fulfilled");
            }

            _dependencies[typeof(T)] = t;
        }
    }
}