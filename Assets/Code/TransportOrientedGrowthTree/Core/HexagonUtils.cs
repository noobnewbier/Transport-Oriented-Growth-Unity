namespace TransportOrientedGrowthTree.Core
{
    public static class HexagonUtils
    {
        public const int NumOfVerticesInHexagon = 6;
        public const int NumOfTrianglesInHexagonalPrism = 20;
        public const int TopHexagonVerticesForSidesStartIndex = NumOfVerticesInHexagon * 2;
        public const int BottomHexagonVerticesForSidesStartIndex = TopHexagonVerticesForSidesStartIndex + NumOfVerticesInHexagon;
        public const int TopHexagonTrianglesStartIndex = 0;
        public const int BottomHexagonTrianglesStartIndex = NumOfTrianglesInHexagon * 3  + TopHexagonTrianglesStartIndex;
        public const int NumOfTrianglesInHexagon = 4;
        public const int PrismSidesStartIndex = BottomHexagonTrianglesStartIndex + NumOfTrianglesInHexagon * 3;
        public const int NumOfTrianglesInPrismSides = 12;
    }
}