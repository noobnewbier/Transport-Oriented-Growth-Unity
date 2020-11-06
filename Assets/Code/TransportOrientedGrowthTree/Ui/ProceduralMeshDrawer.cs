using TransportOrientedGrowthTree.Core;
using UnityEngine;

namespace TransportOrientedGrowthTree.Ui
{
    public class ProceduralMeshDrawer : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;

        public void DrawMesh(MeshData meshData)
        {
            var mesh = meshFilter.mesh;

            mesh.vertices = meshData.Vertices;
            mesh.triangles = meshData.Triangles;
            if (meshData.HasCustomNormal)
                mesh.normals = meshData.Normals;
            else
                mesh.RecalculateNormals();
        }
    }
}