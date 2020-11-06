using UnityEngine;

namespace TransportOrientedGrowthTree.Core
{
    public struct RegularHexagon
    {
        private readonly Vector3[] _hexCorners;

        public RegularHexagon(float hexInnerRadius, float height)
        {
            var hexInnerRadius1 = hexInnerRadius;

            var outerRadius = hexInnerRadius1 / 0.86602540378f;
            _hexCorners = new[]
            {
                new Vector3(0f, height, outerRadius),
                new Vector3(hexInnerRadius1, height, 0.5f * outerRadius),
                new Vector3(hexInnerRadius1, height, -0.5f * outerRadius),
                new Vector3(0f, height, -outerRadius),
                new Vector3(-hexInnerRadius1, height, -0.5f * outerRadius),
                new Vector3(-hexInnerRadius1, height, 0.5f * outerRadius)
            };
        }

        // 0.866025 -> sqrt(3) / 2, read https://catlikecoding.com/unity/tutorials/hex-map/part-1/, session "about hexagons" for details

        //Origin from center, begin from top, rotate clockwise
        public Vector3[] GetHexCorners() => _hexCorners;
    }
}