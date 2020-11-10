using TransportOrientedGrowthTree.Core;
using UnityEngine;

namespace TransportOrientedGrowthTree.Di.Composers
{
    public class Composer : MonoBehaviour
    {
        private IDependenciesProvider _cache;

        public IDependenciesProvider DependenciesProvider => _cache ?? (_cache = ComposeDependencies());

        private static IDependenciesProvider ComposeDependencies()
        {
            var meshDataBuilder = new MeshDataBuilder();
            var meshDataDirector = new MeshDataDirector(meshDataBuilder);
            var toReturn = new DependenciesProvider();

            toReturn.AddT<IMeshDataBuilder>(meshDataBuilder);
            toReturn.AddT<IMeshDataDirector>(meshDataDirector);

            return toReturn;
        }
    }
}