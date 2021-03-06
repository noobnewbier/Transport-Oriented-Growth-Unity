﻿using UnityEngine;

namespace TransportOrientedGrowthTree.Core
{

    public class GrowthModel
    {
        public float NutritionRatio { get; }
        public float SplitDecay { get; }
        public float InversedDirectedness { get; }
        public int ChildDirectionAccuracyInDepth { get; }
        public float ChildDirectionRatio { get; }
        public float BranchSpread { get; }
        public float MinLengthToSplit { get; }
        public int MinDepthForLeafToAppear { get; }
        public Vector3 LeafSpread { get; }
        public int LeafCount { get; }

        public GrowthModel(float nutritionRatio,
                           float splitDecay,
                           float inversedDirectedness,
                           int childDirectionAccuracyInDepth,
                           float childDirectionRatio,
                           float branchSpread,
                           float minLengthToSplit,
                           int minDepthForLeafToAppear,
                           Vector3 leafSpread,
                           int leafCount)
        {
            NutritionRatio = nutritionRatio;
            SplitDecay = splitDecay;
            InversedDirectedness = inversedDirectedness;
            ChildDirectionAccuracyInDepth = childDirectionAccuracyInDepth;
            ChildDirectionRatio = childDirectionRatio;
            BranchSpread = branchSpread;
            MinLengthToSplit = minLengthToSplit;
            MinDepthForLeafToAppear = minDepthForLeafToAppear;
            LeafSpread = leafSpread;
            LeafCount = leafCount;
        }
    }
}