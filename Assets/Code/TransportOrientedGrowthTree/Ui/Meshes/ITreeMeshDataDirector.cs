using JetBrains.Annotations;
using TransportOrientedGrowthTree.Core;
using UnityEngine;
using Tree = TransportOrientedGrowthTree.Core.Tree;

namespace TransportOrientedGrowthTree.Ui.Meshes
{
    public interface ITreeMeshDataDirector
    {
        MeshData CreateTreeMeshFromData(Tree tree);
    }

    public class TreeMeshDataDirector : ITreeMeshDataDirector
    {
        private readonly IMeshDataBuilder _meshDataBuilder;
        private readonly IPrimitivesMeshDataDirector _primitivesMeshDataDirector;

        public TreeMeshDataDirector(IMeshDataBuilder meshDataBuilder, IPrimitivesMeshDataDirector primitivesMeshDataDirector)
        {
            _meshDataBuilder = meshDataBuilder;
            _primitivesMeshDataDirector = primitivesMeshDataDirector;
        }

        public MeshData CreateTreeMeshFromData(Tree tree)
        {
            _meshDataBuilder.Reset();
            
            AppendMeshFromBranch(tree.Root, Vector3.zero);

            return _meshDataBuilder.Build();
        }

        private void AppendMeshFromBranch([CanBeNull] Branch branch, Vector3 branchStartingPoint)
        {
            if (branch == null)
            {
                return;
            }

            var branchEndingPoint = branchStartingPoint + branch.ToDirection * branch.Length;
            //todo: rotation is incorrect?
            var mainBranchMesh = _primitivesMeshDataDirector.CreateHexPrismSides(
                branchEndingPoint,
                branch.Radius,
                Quaternion.Euler(branch.ToDirection), //todo:  might not actually be euler :)
                branchStartingPoint,
                branch.Radius,
                Quaternion.Euler(branch.FromDirection)
            );

            _meshDataBuilder.Append(mainBranchMesh);
            AppendMeshFromBranch(branch.ChildA, branchEndingPoint);
            // ReSharper disable once TailRecursiveCall : I am too lazy to do it myself and rider won't work this out correctly
            AppendMeshFromBranch(branch.ChildB, branchEndingPoint);
        }
    }
}