using System;

namespace TransportOrientedGrowthTree.Core
{
    public static class LambdaUtils
    {
        // Y-combinator
        public static Func<TA, TR> Y<TA, TR>(Func<Func<TA, TR>, Func<TA, TR>> f)
        {
            Func<TA, TR> g = null;
            g = f(a => g(a));
            return g;
        }
    }
}