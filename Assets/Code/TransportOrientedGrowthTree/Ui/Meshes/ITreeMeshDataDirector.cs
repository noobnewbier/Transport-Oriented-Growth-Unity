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
        private readonly IHexTubeMeshDataDirector _hexTubeMeshDataDirector;

        public TreeMeshDataDirector(IMeshDataBuilder meshDataBuilder, IHexTubeMeshDataDirector hexTubeMeshDataDirector)
        {
            _meshDataBuilder = meshDataBuilder;
            _hexTubeMeshDataDirector = hexTubeMeshDataDirector;
        }

        public MeshData CreateTreeMeshFromData(Tree tree)
        {
            _meshDataBuilder.Reset();

            AppendMeshFromBranch(tree.Root, Vector3.zero);

            return _meshDataBuilder.Build();
        }

        private void AppendMeshFromBranch([CanBeNull] Branch branch, Vector3 branchStartingPoint)
        {
            if (branch == null) return;

            var branchEndingPoint = branchStartingPoint + branch.ToDirection * branch.Length;
            AppendMeshFromMainBranch(branch, branchStartingPoint, branchEndingPoint);
            AppendMeshFromBranch(branch.ChildA, branchEndingPoint);
            // ReSharper disable once TailRecursiveCall : I am too lazy to do it myself and rider won't work this out correctly
            AppendMeshFromBranch(branch.ChildB, branchEndingPoint);
        }

        //todo: rotation is incorrect?
        private void AppendMeshFromMainBranch(Branch branch, Vector3 branchStartingPoint, Vector3 branchEndingPoint)
        {
            if (branch.IsLeaf)
                AppendLeafBranchMesh(branch, branchStartingPoint, branchEndingPoint);
            else
                AppendParentBranchMesh(branch, branchStartingPoint, branchEndingPoint);
        }

        private void AppendLeafBranchMesh(Branch branch, Vector3 branchStartingPoint, Vector3 branchEndingPoint)
        {
            var mainBranchMesh = _hexTubeMeshDataDirector.CreateHexTube(
                new MeshConnectorData(
                    branchEndingPoint,
                    branch.Radius,
                    Quaternion.Euler(branch.ToDirection) //todo:  might not actually be euler :)
                ),
                new MeshConnectorData(
                    branchStartingPoint,
                    branch.Radius,
                    Quaternion.Euler(branch.FromDirection)
                )
            );
            _meshDataBuilder.Append(mainBranchMesh);
        }

        private void AppendParentBranchMesh(Branch branch, Vector3 branchStartingPoint, Vector3 branchEndingPoint)
        {
            if (branch.ChildA != null)
            {
                var toChildAMesh = _hexTubeMeshDataDirector.CreateHexTube(
                    new MeshConnectorData(
                        branchEndingPoint,
                        branch.ChildA.Radius,
                        Quaternion.Euler(branch.ChildA.FromDirection) //todo:  might not actually be euler :)
                    ),
                    new MeshConnectorData(
                        branchStartingPoint,
                        branch.Radius,
                        Quaternion.Euler(branch.FromDirection)
                    )
                );
                _meshDataBuilder.Append(toChildAMesh);
            }

            // ReSharper disable once InvertIf : Improve readability
            if (branch.ChildB != null)
            {
                var toChildBMesh = _hexTubeMeshDataDirector.CreateHexTube(
                    new MeshConnectorData(
                        branchEndingPoint,
                        branch.ChildB.Radius,
                        Quaternion.Euler(branch.ChildB.FromDirection) //todo:  might not actually be euler :)
                    ),
                    new MeshConnectorData(
                        branchStartingPoint,
                        branch.Radius,
                        Quaternion.Euler(branch.FromDirection)
                    )
                );
                _meshDataBuilder.Append(toChildBMesh);
            }
        }
    }
}