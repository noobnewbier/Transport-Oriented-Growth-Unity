using System.Linq;
using g3;
using UnityEngine;

namespace TransportOrientedGrowthTree.Ui.Meshes
{
    public interface IMeshDataSimplifyingService
    {
        MeshData Simplify(MeshData meshData, int numCells = 128, int targetVerticesCount = 10000);
    }

    //http://www.gradientspace.com/tutorials/2017/11/21/signed-distance-fields-tutorial
    public class MeshDataSimplifyingService : IMeshDataSimplifyingService
    {
        public MeshData Simplify(MeshData meshData, int numCells = 128, int targetVerticesCount = 10000)
        {
            var vertices = meshData.Vertices.Select(v => new Vector3f(v.x, v.y, v.z));
            var normals = meshData.Normals?.Select(v => new Vector3f(v.x, v.y, v.z));

            var mesh = DMesh3Builder.Build(vertices, meshData.Triangles, normals);

            mesh = VoxelizeMesh(numCells, mesh);
            mesh = ReduceVertexCount(targetVerticesCount, mesh);
            mesh = CreateCompactMesh(mesh);

            return new MeshData(
                mesh.Vertices().Select(v => new Vector3((float) v.x, (float) v.y, (float) v.z)).ToArray(),
                mesh.Triangles().SelectMany(i3 => i3.array).ToArray()
            );
        }

        private static DMesh3 CreateCompactMesh(DMesh3 mesh)
        {
            mesh = new DMesh3(mesh, true);
            return mesh;
        }

        private static DMesh3 ReduceVertexCount(int targetVerticesCount, DMesh3 mesh)
        {
            var reducer = new Reducer(mesh);
            reducer.ReduceToVertexCount(targetVerticesCount);

            return reducer.Mesh;
        }

        private static DMesh3 VoxelizeMesh(int numCells, DMesh3 mesh)
        {
            var cellSize = mesh.CachedBounds.MaxDim / numCells;
            var sdf = new MeshSignedDistanceGrid(mesh, cellSize);
            sdf.Compute();

            var iso = new DenseGridTrilinearImplicit(sdf.Grid, sdf.GridOrigin, sdf.CellSize);
            var marchingCubesOperation = new MarchingCubes {Implicit = iso, Bounds = mesh.CachedBounds};
            marchingCubesOperation.CubeSize = marchingCubesOperation.Bounds.MaxDim / numCells;
            marchingCubesOperation.Bounds.Expand(2.5 * marchingCubesOperation.CubeSize);
            marchingCubesOperation.Generate();
            mesh = marchingCubesOperation.Mesh;

            return mesh;
        }
    }
}