using System;
using UnityEngine;

namespace TransportOrientedGrowthTree.Core
{
    public class TogTreeMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private GrowthModelScriptable? growthModelScriptable;
        [SerializeField] private float growthRate;
        private TogTree? _togTree;

        public TogTree TogTree
        {
            get
            {
                if (growthModelScriptable is { }) return _togTree ??= new TogTree(growthModelScriptable.ToModel());

                throw new InvalidOperationException($"{nameof(growthModelScriptable)} is null");
            }
        }

        public void Grow()
        {
            TogTree.Grow(growthRate);
        }
    }
}