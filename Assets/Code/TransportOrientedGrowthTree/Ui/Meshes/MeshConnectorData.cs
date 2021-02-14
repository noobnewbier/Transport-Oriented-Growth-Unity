using UnityEngine;

namespace TransportOrientedGrowthTree.Ui.Meshes
{
    public class MeshConnectorData
    {
        public Vector3 Center { get; }
        public float InnerRadius { get; }
        public Quaternion Orientation { get; }

        public MeshConnectorData(Vector3 center, float innerRadius, Quaternion orientation)
        {
            Center = center;
            InnerRadius = innerRadius;
            Orientation = orientation;
        }
    }
}