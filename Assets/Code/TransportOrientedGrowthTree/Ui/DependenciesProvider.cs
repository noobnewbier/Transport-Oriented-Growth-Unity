using System;
using System.Collections.Generic;

namespace TransportOrientedGrowthTree.Ui
{
    public interface IDependenciesProvider
    {
        bool TryGet<T>(Type type, out T t);
        void AddTSingleton<T>(T t) where T : class;
        void AddTFactory<T>(Func<T> tFactory) where T : class;
        T GetFromSingleton<T>(Type type);
        T GetFromFactories<T>(Type type);
    }

    public class DependenciesProvider : IDependenciesProvider
    {
        private readonly IDictionary<Type, Func<object>> _factories;
        private readonly IDictionary<Type, object> _singletons;

        public DependenciesProvider()
        {
            _singletons = new Dictionary<Type, object>();
            _factories = new Dictionary<Type, Func<object>>();
        }

        public bool TryGet<T>(Type type, out T t)
        {
            if (TryGetFromSingleton(type, out t)) return true;
            if (TryGetFromFactories(type, out t)) return true;

            t = default;
            return false;
        }

        public T GetFromSingleton<T>(Type type) => (T) _singletons[type];

        public T GetFromFactories<T>(Type type) => (T) _factories[type].Invoke();

        public void AddTSingleton<T>(T t) where T : class
        {
            if (_singletons.ContainsKey(typeof(T))) throw new ArgumentException($"{typeof(T).FullName} already has its dependencies fulfilled");

            _singletons[typeof(T)] = t;
        }

        //todo: consider use IFactory(what we have in Noneb)
        public void AddTFactory<T>(Func<T> tFactory) where T : class
        {
            if (_factories.ContainsKey(typeof(T))) throw new ArgumentException($"{typeof(T).FullName} already has its dependencies fulfilled");

            _factories[typeof(T)] = tFactory;
        }

        private bool TryGetFromSingleton<T>(Type type, out T t)
        {
            if (_singletons.ContainsKey(type))
            {
                t = GetFromSingleton<T>(type);
                return true;
            }

            t = default;
            return false;
        }

        private bool TryGetFromFactories<T>(Type type, out T t)
        {
            if (_factories.ContainsKey(type))
            {
                t = GetFromFactories<T>(type);
                return true;
            }

            t = default;
            return false;
        }
    }
}