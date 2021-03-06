﻿using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityUtils.Constants;

namespace TransportOrientedGrowthTree.Core
{
    [CreateAssetMenu(fileName = nameof(GrowthModel), menuName = MenuName.Data + nameof(GrowthModel))]
    public class GrowthModelScriptable : ScriptableObject
    {
        [SerializeField] private float branchSpread = 0.45f;
        [SerializeField] private int childDirectionAccuracyInDepth = 2;
        [SerializeField] private float childDirectionRatio = 0.6f;
        [SerializeField] private float inversedDirectedness = 0.5f;
        [SerializeField] private float minLengthToSplit = 2.5f;
        [SerializeField] private float nutritionRatio = 0.3f;
        [SerializeField] private float splitDecay = 1E-2f;
        [SerializeField] private int minDepthForLeafToAppear;
        [SerializeField] private Vector3 leafSpread;
        [SerializeField] private int leafCount;
        
        
        

        public GrowthModel ToModel() =>
            new GrowthModel(
                nutritionRatio,
                splitDecay,
                inversedDirectedness,
                childDirectionAccuracyInDepth,
                childDirectionRatio,
                branchSpread,
                minLengthToSplit,
                minDepthForLeafToAppear,
                leafSpread,
                leafCount
            );
    }
}