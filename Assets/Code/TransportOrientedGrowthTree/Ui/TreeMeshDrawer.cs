using TransportOrientedGrowthTree.Core;
using TransportOrientedGrowthTree.Di;
using TransportOrientedGrowthTree.Ui.Meshes;
using UnityEngine;

namespace TransportOrientedGrowthTree.Ui
{
    public class TreeMeshDrawer : MonoBehaviour, IInjectable
    {
        private ITreeMeshDataDirector _treeMeshDataDirector;
        [SerializeField] private MeshDrawer meshDrawer;
        [SerializeField] private TreeMonoBehaviour treeMonoBehaviour;

        [Inject]
        public void Inject(ITreeMeshDataDirector treeMeshDataDirector)
        {
            _treeMeshDataDirector = treeMeshDataDirector;
        }

        public void GrowTree()
        {
            treeMonoBehaviour.Grow();
        }

        public void Draw()
        {
            meshDrawer.DrawMesh(_treeMeshDataDirector.CreateTreeMeshFromData(treeMonoBehaviour.Tree));
        }
    }
}