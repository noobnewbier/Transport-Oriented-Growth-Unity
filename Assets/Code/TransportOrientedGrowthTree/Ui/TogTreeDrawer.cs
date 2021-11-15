using System;
using System.Linq;
using TransportOrientedGrowthTree.Core;
using TransportOrientedGrowthTree.Di;
using TransportOrientedGrowthTree.Ui.Meshes;
using UnityEngine;
using UnityEngine.Serialization;

namespace TransportOrientedGrowthTree.Ui
{
    public class TogTreeDrawer : MonoBehaviour, IInjectable
    {
        [SerializeField] private MeshDrawer meshDrawer = null!;
        [SerializeField] private new ParticleSystem particleSystem = null!;
        

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
                var togTree = togTreeMonoBehaviour.TogTree;
                
                _currentMeshData = _treeMeshDataDirector.CreateTreeMeshFromData(togTree);
                meshDrawer.DrawMesh(_currentMeshData);

                var leafPositions = togTree.GetLeafPositions();

                //ignore the allocation problem - it is not needed now
                var particles = leafPositions.Select(p => new ParticleSystem.Particle
                    {
                        position = p,
                        remainingLifetime = float.MaxValue,
                        startSize = particleSystem.main.startSize.constantMax,
                        startColor = Color.green
                    }
                ).ToArray();
                
                Debug.Log($"Particles Count: {particles.Length}");
                particleSystem.SetParticles(particles);


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