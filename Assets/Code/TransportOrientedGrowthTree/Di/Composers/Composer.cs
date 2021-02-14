using System;
using TransportOrientedGrowthTree.Ui;
using TransportOrientedGrowthTree.Ui.Meshes;
using UnityEngine;

namespace TransportOrientedGrowthTree.Di.Composers
{
    public class Composer : MonoBehaviour
    {
        private readonly Lazy<IDependenciesProvider> _cache = new Lazy<IDependenciesProvider>(() => new DependenciesProvider());

        public IDependenciesProvider DependenciesProvider
        {
            get
            {
                if (!_cache.IsValueCreated) ComposeDependencies();

                return _cache.Value;
            }
        }

        private void ComposeDependencies()
        {
            ComposeFactories(_cache.Value);
            ComposeSingletons(_cache.Value);
        }

        private static void ComposeSingletons(IDependenciesProvider dependenciesProvider)
        {
            AddPrimitivesMeshDataDirector(dependenciesProvider);
            AddTreeMeshDirector(dependenciesProvider);
        }

        private static void AddTreeMeshDirector(IDependenciesProvider dependenciesProvider)
        {
            var meshDataBuilder = dependenciesProvider.GetFromFactories<IMeshDataBuilder>(typeof(IMeshDataBuilder));
            var treeMeshDataDirector = new TreeMeshDataDirector(
                meshDataBuilder,
                dependenciesProvider.GetFromSingleton<IHexTubeMeshDataDirector>(typeof(IHexTubeMeshDataDirector))
            );

            dependenciesProvider.AddTSingleton<ITreeMeshDataDirector>(treeMeshDataDirector);
        }

        private static void AddPrimitivesMeshDataDirector(IDependenciesProvider dependenciesProvider)
        {
            var meshBuilder = dependenciesProvider.GetFromFactories<IMeshDataBuilder>(typeof(IMeshDataBuilder));
            dependenciesProvider.AddTSingleton<IHexTubeMeshDataDirector>(new HexTubeMeshDataDirector(meshBuilder));
        }

        private static void ComposeFactories(IDependenciesProvider dependenciesProvider)
        {
            AddMeshDataBuilderFactory(dependenciesProvider);
        }

        private static void AddMeshDataBuilderFactory(IDependenciesProvider dependenciesProvider)
        {
            dependenciesProvider.AddTFactory<IMeshDataBuilder>(() => new MeshDataBuilder());
        }
    }
}