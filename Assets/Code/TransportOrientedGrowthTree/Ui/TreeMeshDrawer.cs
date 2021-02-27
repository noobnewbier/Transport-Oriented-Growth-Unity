using TransportOrientedGrowthTree.Core;
using TransportOrientedGrowthTree.Di;
using TransportOrientedGrowthTree.Ui.Meshes;
using UnityEngine;
using UnityEngine.Serialization;

namespace TransportOrientedGrowthTree.Ui
{
    public class TreeMeshDrawer : MonoBehaviour, IInjectable
    {
        [SerializeField] private MeshDrawer? meshDrawer;

        [FormerlySerializedAs("treeMonoBehaviour")] [SerializeField]
        private TogTreeMonoBehaviour? togTreeMonoBehaviour;

        private ITreeMeshDataDirector _treeMeshDataDirector = null!;
        private IMeshDataSimplifyingService _meshDataSimplifyingService = null!;
        private MeshData? _currentMeshData;

        [Inject]
        public void Inject(ITreeMeshDataDirector treeMeshDataDirector, IMeshDataSimplifyingService meshDataSimplifyingService)
        {
            _treeMeshDataDirector = treeMeshDataDirector;
            _meshDataSimplifyingService = meshDataSimplifyingService;
        }

        public void GrowTree()
        {
            if (togTreeMonoBehaviour != null)
            {
                togTreeMonoBehaviour.Grow();
            }
        }

        public void Draw()
        {
            if (togTreeMonoBehaviour != null && meshDrawer != null)
            {
                _currentMeshData = _treeMeshDataDirector.CreateTreeMeshFromData(togTreeMonoBehaviour.TogTree);
                meshDrawer.DrawMesh(_currentMeshData);
            }
        }

        public void SimplifyMesh(int cellsCount, int trianglesCount)
        {
            if (_currentMeshData != null && meshDrawer != null)
            {
                var simplifiedMeshData = _meshDataSimplifyingService.Simplify(_currentMeshData, cellsCount, trianglesCount);
                meshDrawer.DrawMesh(simplifiedMeshData);
            }
        }
    }
}