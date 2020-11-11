namespace TransportOrientedGrowthTree.Core
{
    public struct GrowthModel
    {
        public float GrowthRate => 1.0f;
        public float PassRatio => 0.3f;
        public float SplitDecay => 1E-2f;
        public float Directedness => 0.5f;
        public int LocalDepth => 2;
    }
}