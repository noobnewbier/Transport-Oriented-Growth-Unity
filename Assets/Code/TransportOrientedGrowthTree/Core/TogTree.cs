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

            foreach (var position in branch.GetLocalLeafPositions())
            {
                var translatedPosition = basePosition + position;

                yield return translatedPosition;
            }

            var parentEndpoint = branch.Length * branch.ToDirection;
            var childBase = basePosition + parentEndpoint;

            foreach (var vector3 in GetLeafPositions(branch.ChildA, childBase)) yield return vector3;
            foreach (var vector3 in GetLeafPositions(branch.ChildB, childBase)) yield return vector3;
        }
    }
}