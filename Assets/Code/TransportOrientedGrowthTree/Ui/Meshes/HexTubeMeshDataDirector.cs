using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TransportOrientedGrowthTree.Ui.Meshes
{
    public interface IHexTubeMeshDataDirector
    {
        MeshData CreateHexTube(MeshConnectorData top, MeshConnectorData bottom);
    }

    public class HexTubeMeshDataDirector : IHexTubeMeshDataDirector
    {
        private const float ThresholdToDrawTube = 0.0001f;
        private readonly IMeshDataBuilder _meshDataBuilder;

        public HexTubeMeshDataDirector(IMeshDataBuilder meshDataBuilder)
        {
            _meshDataBuilder = meshDataBuilder;
        }

        public MeshData CreateHexTube(MeshConnectorData top, MeshConnectorData bottom)
        {
            _meshDataBuilder.Reset();

            AppendHexTube(top, bottom);

            return _meshDataBuilder.Build();
        }

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

        private MeshData CreateQuad(Vector3 topLeft,
                                    Vector3 topRight,
                                    Vector3 bottomLeft,
                                    Vector3 bottomRight)
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

        private void AppendHexTube(MeshConnectorData top, MeshConnectorData bottom)
        {
            var topCenter = top.Center;
            var topInnerRadius = top.InnerRadius;
            var topOrientation = top.Orientation;
            var bottomCenter = bottom.Center;
            var bottomInnerRadius = bottom.InnerRadius;
            var bottomOrientation = bottom.Orientation;

            if (Vector3.Distance(topCenter, bottomCenter) < ThresholdToDrawTube) return;

            var topHexRing = GetHexRingVertices(topInnerRadius, topCenter)
                .Select(v => RotateVectorAroundPivot(topCenter, topOrientation, v))
                .ToArray();
            var bottomHexRing = GetHexRingVertices(bottomInnerRadius, bottomCenter)
                .Select(v => RotateVectorAroundPivot(bottomCenter, bottomOrientation, v))
                .ToArray();

            for (var i = 0; i < 6; i++)
            {
                var rightIndex = i;
                var leftIndex = (i + 1) % 6;

                _meshDataBuilder.Append(
                    CreateQuad(
                        topHexRing[leftIndex],
                        topHexRing[rightIndex],
                        bottomHexRing[leftIndex],
                        bottomHexRing[rightIndex]
                    )
                );
            }
        }

        private static Vector3 RotateVectorAroundPivot(Vector3 pivot, Quaternion rotation, Vector3 v) => rotation * (v - pivot) + pivot;

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
    }
}