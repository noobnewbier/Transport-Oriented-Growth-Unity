using System;
using System.Collections.Generic;
using UnityEngine;

namespace TransportOrientedGrowthTree.Core
{
    public class TogTree : IDisposable
    {
        public Branch Root { get; }

        public TogTree(GrowthModel growthModel)
        {
            Root = new Branch(growthModel);
        }

        public void Dispose()
        {
            Root.Dispose();
        }

        public void Grow(float feed)
        {
            Root.Grow(feed);
        }

        public IEnumerable<Vector3> GetLeafPositions() => GetLeafPositions(Root, Vector3.zero);

        private IEnumerable<Vector3> GetLeafPositions(Branch? branch, Vector3 basePosition)
        {
            if (branch == null) yield break;

            foreach (var vector3 in GetTranslatedLeafPosition(branch, basePosition)) yield return vector3;

            var branchEndpoint = branch.Length * branch.ToDirection;
            var childBase = basePosition + branchEndpoint;

            foreach (var vector3 in GetTranslatedLeafPosition(branch.ChildA, childBase)) yield return vector3;
            foreach (var vector3 in GetTranslatedLeafPosition(branch.ChildB, childBase)) yield return vector3;
        }

        private static IEnumerable<Vector3> GetTranslatedLeafPosition(Branch? branch, Vector3 center)
        {
            if (branch == null) yield break;

            foreach (var position in branch.GetLocalLeafPositions())
            {
                var translatedPosition = center + position;

                yield return translatedPosition;
            }
        }
    }
}