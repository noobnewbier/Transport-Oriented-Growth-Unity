﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TransportOrientedGrowthTree.Ui.Meshes
{
    public interface IMeshDataDirector
    {
        MeshData CreateHexPrismSides(Vector3 topCenter,
                                     float topInnerRadius,
                                     Quaternion topOrientation,
                                     Vector3 bottomCenter,
                                     float bottomInnerRadius,
                                     Quaternion bottomOrientation);

        MeshData GetQuad(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight);
        MeshData CreateTriangle(Vector3 a, Vector3 b, Vector3 c);
        MeshData CreateHexagon(float hexInnerRadius, Vector3 center);
    }

    public class MeshDataDirector : IMeshDataDirector
    {
        private readonly IMeshDataBuilder _meshDataBuilder;

        public MeshDataDirector(IMeshDataBuilder meshDataBuilder)
        {
            _meshDataBuilder = meshDataBuilder;
        }

        public MeshData CreateHexPrismSides(Vector3 topCenter,
                                            float topInnerRadius,
                                            Quaternion topOrientation,
                                            Vector3 bottomCenter,
                                            float bottomInnerRadius,
                                            Quaternion bottomOrientation)
        {
            var topHexRing = GetHexRingVertices(topInnerRadius, topCenter)
                .Select(v => RotateVectorAroundPivot(topCenter, topOrientation, v))
                .ToArray();
            var bottomHexRing = GetHexRingVertices(bottomInnerRadius, bottomCenter)
                .Select(v => RotateVectorAroundPivot(bottomCenter, bottomOrientation, v))
                .ToArray();

            _meshDataBuilder.Reset();

            for (var i = 0; i < 6; i++)
            {
                var rightIndex = i;
                var leftIndex = (i + 1) % 6;

                _meshDataBuilder.Append(
                    GetQuad(
                        topHexRing[leftIndex],
                        topHexRing[rightIndex],
                        bottomHexRing[leftIndex],
                        bottomHexRing[rightIndex]
                    )
                );
            }

            return _meshDataBuilder.Build();
        }

        private static Vector3 RotateVectorAroundPivot(Vector3 pivot, Quaternion rotation, Vector3 v) => rotation * (v - pivot) + pivot;

        public MeshData CreateHexagon(float hexInnerRadius, Vector3 center)
        {
            var vertices = GetHexRingVertices(hexInnerRadius, center).ToList();
            vertices.Add(center);
            return new MeshData(
                vertices.ToArray(),
                new[]
                {
                    0, 1, 6,
                    1, 2, 6,
                    6, 2, 3,
                    6, 3, 4,
                    6, 4, 5,
                    0, 6, 5
                }
            );
        }


        private static IEnumerable<Vector3> GetHexRingVertices(float hexInnerRadius, Vector3 center)
        {
            var outerRadius = hexInnerRadius / 0.86602540378f;

            return new[]
            {
                new Vector3(center.x, center.y, outerRadius + center.z),
                new Vector3(hexInnerRadius + center.x, center.y, 0.5f * outerRadius + center.z),
                new Vector3(hexInnerRadius + center.x, center.y, -0.5f * outerRadius + center.z),
                new Vector3(center.x, center.y, -outerRadius + center.z),
                new Vector3(-hexInnerRadius + center.x, center.y, -0.5f * outerRadius + center.z),
                new Vector3(-hexInnerRadius + center.x, center.y, 0.5f * outerRadius + center.z)
            };
        }

        public MeshData GetQuad(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight)
        {
            return new MeshData(
                new[] {bottomLeft, bottomRight, topLeft, topRight},
                new[]
                {
                    0, 2, 1,
                    2, 3, 1
                }
            );
        }

        public MeshData CreateTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            return new MeshData(
                new[] {a, b, c},
                new[] {0, 1, 2}
            );
        }
    }
}