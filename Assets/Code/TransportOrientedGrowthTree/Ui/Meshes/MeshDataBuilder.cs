using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TransportOrientedGrowthTree.Ui.Meshes
{
    public interface IMeshDataBuilder
    {
        MeshDataBuilder Append(MeshData meshData);
        MeshData Build();
        void Reset();
    }

    public class MeshDataBuilder : IMeshDataBuilder
    {
        private readonly List<Vector3> _normals;
        private readonly List<int> _triangles;
        private readonly List<Vector3> _vertices;
        private bool _hasCustomNormal;

        public MeshDataBuilder(MeshData data)
        {
            _vertices = data.Vertices.ToList();
            _triangles = data.Triangles.ToList();
            _normals = data.Normals?.ToList() ?? new List<Vector3>(_vertices.Count);
            _hasCustomNormal = data.HasCustomNormal;
        }

        public MeshDataBuilder()
        {
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _normals = new List<Vector3>();
            _hasCustomNormal = false;
        }

        public MeshDataBuilder Append(MeshData meshData)
        {
            var originalVerticesLength = _vertices.Count;
            _vertices.AddRange(meshData.Vertices);
            _triangles.AddRange(meshData.Triangles.Select(i => i + originalVerticesLength));

            if (meshData.HasCustomNormal)
            {
                _hasCustomNormal = true;
                _normals.AddRange(
                    meshData.Normals ?? throw new ArgumentOutOfRangeException($"{meshData} claims to have custom normals but normals field is null")
                );
            }
            else
            {
                _normals.AddRange(Enumerable.Repeat(Vector3.zero, meshData.Vertices.Length));
            }

            return this;
        }

        public MeshData Build() => new MeshData(_vertices.ToArray(), _triangles.ToArray(), _hasCustomNormal ? _normals.ToArray() : null);

        public void Reset()
        {
            _hasCustomNormal = false;
            _normals.Clear();
            _triangles.Clear();
            _vertices.Clear();
        }
    }
}