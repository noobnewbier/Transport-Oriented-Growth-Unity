using System;

namespace TransportOrientedGrowthTree.Core
{
    public class Tree : IDisposable
    {
        public Branch Root { get; }

        public Tree(GrowthModel growthModel)
        {
            Root = new Branch(growthModel);
        }

        public void Grow(float feed)
        {
            Root.Grow(feed);
        }

        public void Dispose()
        {
            Root.Dispose();
        }
    }
}