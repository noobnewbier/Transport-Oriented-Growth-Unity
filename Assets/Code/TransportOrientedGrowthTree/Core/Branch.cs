using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TransportOrientedGrowthTree.Core
{
    //todo: might need to double 
    public class Branch : IDisposable
    {
        private readonly int _depth;
        private readonly GrowthModel _growthModel;
        private readonly int _id;
        private readonly Branch _parent;

        private bool _isLeaf;
        private float _crossSectionArea = 0.1f;

        public Vector3 ToDirection { get; }
        public Vector3 FromDirection { get; }

        [CanBeNull] public Branch ChildA { get; private set; }

        [CanBeNull] public Branch ChildB { get; private set; }

        public float Length { get; private set; }

        public float Radius { get; private set; }


        public Branch(GrowthModel growthModel)
        {
            _growthModel = growthModel;
            ToDirection = Vector3.up;
            FromDirection = Vector3.up;
            _isLeaf = true;
        }

        public Branch(Branch parent, int id, Vector3 toDirection, Vector3 fromDirection)
        {
            _growthModel = parent._growthModel;
            _depth = parent._depth + 1;
            _parent = parent;
            _id = id;
            ToDirection = toDirection;
            FromDirection = fromDirection;
            _isLeaf = true;
        }

        private float SplitRequirementOnLength => _growthModel.MinLengthToSplit * Mathf.Exp(-_growthModel.SplitDecay * _depth);

        public void Dispose()
        {
            ChildA?.Dispose();
            ChildB?.Dispose();
        }

        public void Grow(float feed)
        {
            Radius = Mathf.Sqrt(_crossSectionArea / Mathf.PI);

            if (_isLeaf)
                GrowAsLeaf(feed);
            else
                GrowAsParent(feed);
        }

        private void GrowAsParent(float feed)
        {
            var passRatio = GetPassRatio();

            _crossSectionArea += passRatio * feed / Length;
            feed *= 1 - passRatio;

            if (!HasEnoughFeedForChildren(feed)) return;

            ChildA?.Grow(feed * _growthModel.NutritionRatio);
            ChildB?.Grow(feed * (1 - _growthModel.NutritionRatio));
        }

        private static bool HasEnoughFeedForChildren(float feed) => feed > 1E-5;

        //eventually converge to 0.5 - conserving the cross-section area constraint
        private float GetPassRatio()
        {
            var childACrossSectionArea = ChildA?._crossSectionArea ?? 0f;
            var childBCrossSectionArea = ChildB?._crossSectionArea ?? 0f;

            return (childACrossSectionArea + childBCrossSectionArea) /
                   (childACrossSectionArea + childBCrossSectionArea + _crossSectionArea);
        }

        private void GrowAsLeaf(float feed)
        {
            var growthInLength = Mathf.Pow(feed, 1f / 3f);

            Length += growthInLength;
            feed -= growthInLength * _crossSectionArea;

            _crossSectionArea += feed / Length; // todo: this look suspicious, can be replaced by our "newer formula"

            if (LongEnoughToSplit())
                Split();
        }

        private bool LongEnoughToSplit() => Length > SplitRequirementOnLength;

        private void Split()
        {
            _isLeaf = false;

            var (aDirection, bDirection) = GetDirectionsForChildren();
            ChildA = new Branch(this, 2 * _id, aDirection, ToDirection);
            ChildB = new Branch(this, 2 * _id + 1, bDirection, ToDirection);
        }

        private (Vector3 aDirection, Vector3 bDirection) GetDirectionsForChildren()
        {
            var directionWithHighestLeafDensity = GetDirectionWithHighestLeafDensity(_growthModel.ChildDirectionAccuracyInDepth);
            var normalToSelfDirectionAndHighestLeafDensityPlane = Vector3.Cross(ToDirection, directionWithHighestLeafDensity);
            var randomDirectionFlip = Random.value > 0.5 ? 1f : -1f;

            var aDirection = Vector3.LerpUnclamped(
                    randomDirectionFlip * _growthModel.BranchSpread * normalToSelfDirectionAndHighestLeafDensityPlane,
                    ToDirection,
                    _growthModel.ChildDirectionRatio
                )
                .normalized;
            var bDirection = Vector3.LerpUnclamped(
                    randomDirectionFlip * _growthModel.BranchSpread * -normalToSelfDirectionAndHighestLeafDensityPlane,
                    ToDirection,
                    1 - _growthModel.ChildDirectionRatio
                )
                .normalized;

            return (aDirection, bDirection);
        }

        private Vector3 GetDirectionWithHighestLeafDensity(int searchDepth)
        {
            var noiseVector = GetNoiseVector();

            if (_depth == 0)
                return noiseVector;

            var ancestorNode = this;
            var relativePositionToStartNode = Vector3.zero;
            while (ancestorNode._depth > 0 && searchDepth-- >= 0)
            {
                relativePositionToStartNode += ancestorNode.Length * ancestorNode.ToDirection;
                ancestorNode = ancestorNode._parent;
            }

            //Average relative to ancestor, shifted by rel ( + Noise )
            //todo: noise should be added after this function, the function should return the exact value, maybe same as directedness
            return _growthModel.Directedness * (GetAverageLeafPositionOfChildren(ancestorNode) - relativePositionToStartNode).normalized +
                   (1.0f - _growthModel.Directedness) * noiseVector;
        }

        private Vector3 GetAverageLeafPositionOfChildren([CanBeNull] Branch branch)
        {
            if (branch == null)
            {
                return Vector3.zero;
            }

            var selfAverageLeafPosition = branch.Length * branch.ToDirection;
            var childAAverageLeafPosition = _growthModel.ChildDirectionRatio * GetAverageLeafPositionOfChildren(branch.ChildA);
            var childBAverageLeafPosition = (1.0f - _growthModel.ChildDirectionRatio) * GetAverageLeafPositionOfChildren(branch.ChildB);

            return selfAverageLeafPosition + childAAverageLeafPosition + childBAverageLeafPosition;
        }

        //todo: looks suspicious
        private static Vector3 GetNoiseVector() => new Vector3(Random.value % 100, Random.value % 100, Random.value % 100) / 100 - Vector3.one * 0.5f;
    }
}