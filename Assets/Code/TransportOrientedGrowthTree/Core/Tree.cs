using System;

namespace TransportOrientedGrowthTree.Core
{
    public class Tree : IDisposable
    {
        public Tree(GrowthModel growthModel)
        {
            Root = new Branch(growthModel);
        }

        public Branch Root { get; }

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