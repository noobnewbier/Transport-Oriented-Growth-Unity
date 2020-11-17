using TransportOrientedGrowthTree.Ui;
using TransportOrientedGrowthTree.Ui.Meshes;
using UnityEngine;

namespace TransportOrientedGrowthTree.Di.Composers
{
    public class Composer : MonoBehaviour
    {
        private IDependenciesProvider _cache;

        public IDependenciesProvider DependenciesProvider => _cache ?? (_cache = ComposeDependencies());

        private static IDependenciesProvider ComposeDependencies()
        {
            var toReturn = new DependenciesProvider();

            ComposeFactories(toReturn);
            ComposeSingletons(toReturn);

            return toReturn;
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