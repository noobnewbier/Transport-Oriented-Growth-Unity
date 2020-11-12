using System.Diagnostics;
using UnityEngine;

namespace TransportOrientedGrowthTree.Ui.Meshes
{
    public class MeshDrawer : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;

        public void DrawMesh(MeshData meshData)
        {
            LogMeshData(meshData);
            
            var mesh = meshFilter.mesh;
            
            mesh.Clear();
            
            mesh.vertices = meshData.Vertices;
            mesh.triangles = meshData.Triangles;
            if (meshData.HasCustomNormal)
                mesh.normals = meshData.Normals;
            else
                mesh.RecalculateNormals();
        }

        [Conditional("UNITY_EDITOR")]
        private void LogMeshData(MeshData meshData)
        {
            UnityEngine.Debug.Log($"vertices count: {meshData.Vertices.Length}");
        }
    }
}