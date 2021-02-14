using TransportOrientedGrowthTree.Core;
using TransportOrientedGrowthTree.Di;
using TransportOrientedGrowthTree.Ui.Meshes;
using UnityEngine;
using UnityEngine.Serialization;

namespace TransportOrientedGrowthTree.Ui
{
    public class TreeMeshDrawer : MonoBehaviour, IInjectable
    {
        [SerializeField] private MeshDrawer meshDrawer;

        [FormerlySerializedAs("treeMonoBehaviour")] [SerializeField]
        private TogTreeMonoBehaviour togTreeMonoBehaviour;

        private ITreeMeshDataDirector _treeMeshDataDirector;

        [Inject]
        public void Inject(ITreeMeshDataDirector treeMeshDataDirector)
        {
            _treeMeshDataDirector = treeMeshDataDirector;
        }

        public void GrowTree()
        {
            togTreeMonoBehaviour.Grow();
        }

        public void Draw()
        {
            meshDrawer.DrawMesh(_treeMeshDataDirector.CreateTreeMeshFromData(togTreeMonoBehaviour.TogTree));
        }
    }
}