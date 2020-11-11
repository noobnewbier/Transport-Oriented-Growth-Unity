﻿using JetBrains.Annotations;
using UnityEngine;

namespace TransportOrientedGrowthTree.Ui.Meshes
{
    //consider making everything immutable
    public class MeshData
    {
        public MeshData(Vector3[] vertices, int[] triangles, Vector3[] normals = null)
        {
            Triangles = triangles;
            Vertices = vertices;
            Normals = normals;
        }

        public int[] Triangles { get; }
        public Vector3[] Vertices { get; }
        [CanBeNull] public Vector3[] Normals { get; }
        public bool HasCustomNormal => Normals != null;
    }
}