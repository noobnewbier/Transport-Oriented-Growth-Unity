using TransportOrientedGrowthTree.Di;
using TransportOrientedGrowthTree.Ui;
using TransportOrientedGrowthTree.Ui.Meshes;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Serialization;

namespace TransportOrientedGrowthTree.Core
{
    public class TreeMonoBehaviour : MonoBehaviour, IInjectable
    {
        [SerializeField] private GrowthModelScriptable growthModelScriptable;
        [SerializeField] private MeshDrawer meshDrawer;
        [SerializeField] private float growthRate;

        private Tree _tree;
        private ITreeMeshDataDirector _treeMeshDataDirector;

        [Inject]
        public void Inject(ITreeMeshDataDirector treeMeshDataDirector)
        {
            _treeMeshDataDirector = treeMeshDataDirector;
        }
        
        private void OnEnable()
        {
            _tree = new Tree(growthModelScriptable.ToModel());
        }

        public void Grow()
        {
            _tree.Grow(growthRate);
        }

        public void Draw()
        {
            meshDrawer.DrawMesh(_treeMeshDataDirector.CreateTreeMeshFromData(_tree));
        }
    }
}