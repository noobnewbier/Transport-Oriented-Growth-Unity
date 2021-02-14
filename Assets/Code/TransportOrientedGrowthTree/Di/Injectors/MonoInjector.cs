using System;
using TransportOrientedGrowthTree.Di.Composers;
using UnityEngine;

namespace TransportOrientedGrowthTree.Di.Injectors
{
    public class MonoInjector : MonoBehaviour
    {
        [SerializeField] private Composer? composer;

        private void Awake()
        {
            if (composer is { })
            {
                var injector = new Injector(composer.DependenciesProvider);

                foreach (var rootGo in gameObject.scene.GetRootGameObjects())
                foreach (var injectable in rootGo.GetComponentsInChildren<IInjectable>())
                    injector.InjectToInjectable(injectable);
            }
            else
            {
                throw new InvalidOperationException($"{nameof(composer)} is null");
            }
        }
    }
}