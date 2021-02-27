using UnityEngine;

namespace TransportOrientedGrowthTree.Ui.Meshes
{
    //why do I even need this? should probably just work with the mesh
    public class MeshData
    {
        public int[] Triangles { get; }
        public Vector3[] Vertices { get; }
        public Vector3[]? Normals { get; }
        public bool HasCustomNormal => Normals != null;

        public static MeshData Empty => new MeshData(new Vector3 [0], new int [0]);

        public MeshData(Vector3[] vertices, int[] triangles, Vector3[]? normals = null)
        {
            Triangles = triangles;
            Vertices = vertices;
            Normals = normals;
        }

        public Mesh ToMesh()
        {
            var mesh = new Mesh {vertices = Vertices, triangles = Triangles};

            if (HasCustomNormal)
                mesh.normals = Normals;
            else
                mesh.RecalculateNormals();

            return mesh;
        }
    }
}