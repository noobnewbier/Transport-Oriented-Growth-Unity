﻿namespace TransportOrientedGrowthTree.Core
{
    public class GrowthModel
    {
        public GrowthModel(float nutritionRatio,
                           float splitDecay,
                           float directedness,
                           int childDirectionAccuracyInDepth,
                           float childDirectionRatio,
                           float branchSpread,
                           float minLengthToSplit)
        {
            NutritionRatio = nutritionRatio;
            SplitDecay = splitDecay;
            Directedness = directedness;
            ChildDirectionAccuracyInDepth = childDirectionAccuracyInDepth;
            ChildDirectionRatio = childDirectionRatio;
            BranchSpread = branchSpread;
            MinLengthToSplit = minLengthToSplit;
        }

        //todo: might have confused between nutrition ratio and child direction ratio
        public float NutritionRatio { get; }
        public float SplitDecay { get; }
        public float Directedness { get; }
        public int ChildDirectionAccuracyInDepth { get; }
        public float ChildDirectionRatio { get; }
        public float BranchSpread { get; }
        public float MinLengthToSplit { get; }
    }
}