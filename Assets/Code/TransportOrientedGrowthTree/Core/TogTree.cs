using System;

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
    }
}