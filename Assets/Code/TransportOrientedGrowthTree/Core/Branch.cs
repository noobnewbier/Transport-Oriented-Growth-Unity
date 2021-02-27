using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TransportOrientedGrowthTree.Core
{
    public class Branch : IDisposable
    {
        private readonly int _depth;
        private readonly GrowthModel _growthModel;
        private readonly int _id;
        private readonly Branch? _parent;
        private float _crossSectionArea = 0.1f;

        public bool IsLeaf { get; private set; }
        public Vector3 ToDirection { get; }
        public Vector3 FromDirection { get; }

        public Branch? ChildA { get; private set; }

        public Branch? ChildB { get; private set; }

        public float Length { get; private set; }

        public float Radius { get; private set; }

        private float SplitRequirementOnLength => _growthModel.MinLengthToSplit * Mathf.Exp(-_growthModel.SplitDecay * _depth);


        public Branch(GrowthModel growthModel)
        {
            _growthModel = growthModel;
            ToDirection = Vector3.up;
            FromDirection = Vector3.up;
            IsLeaf = true;
        }

        private Branch(Branch parent,
                       int id,
                       Vector3 toDirection,
                       Vector3 fromDirection)
        {
            _growthModel = parent._growthModel;
            _depth = parent._depth + 1;
            _parent = parent;
            _id = id;
            ToDirection = toDirection;
            FromDirection = fromDirection;
            IsLeaf = true;
        }

        public void Dispose()
        {
            ChildA?.Dispose();
            ChildB?.Dispose();
        }

        public void Grow(float feed)
        {
            Radius = Mathf.Sqrt(_crossSectionArea / Mathf.PI);

            if (IsLeaf)
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

            return (childACrossSectionArea + childBCrossSectionArea) / (childACrossSectionArea + childBCrossSectionArea + _crossSectionArea);
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
            IsLeaf = false;

            var (aDirection, bDirection) = GetDirectionsForChildren();
            ChildA = new Branch(
                this,
                2 * _id,
                aDirection,
                ToDirection
            );
            ChildB = new Branch(
                this,
                2 * _id + 1,
                bDirection,
                ToDirection
            );
        }

        private (Vector3 aDirection, Vector3 bDirection) GetDirectionsForChildren()
        {
            var direction = _depth == 0 ?
                GetNoiseVector() :
                GetDirectionWithHighestLeafDensity(_growthModel.ChildDirectionAccuracyInDepth) +
                (1.0f - _growthModel.Directedness) * GetNoiseVector();

            var normalToSelfDirectionAndHighestLeafDensityPlane = Vector3.Cross(ToDirection, direction);
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
            while (ancestorNode != null && ancestorNode._depth > 0 && searchDepth-- >= 0)
            {
                relativePositionToStartNode += ancestorNode.Length * ancestorNode.ToDirection;
                ancestorNode = ancestorNode._parent;
            }

            //Average relative to ancestor, shifted by relative position
            return _growthModel.Directedness * (GetAverageLeafPositionOfChildren(ancestorNode) - relativePositionToStartNode).normalized;
        }

        private Vector3 GetAverageLeafPositionOfChildren(Branch? branch)
        {
            if (branch == null) return Vector3.zero;

            var selfAverageLeafPosition = branch.Length * branch.ToDirection;
            var childAAverageLeafPosition = _growthModel.ChildDirectionRatio * GetAverageLeafPositionOfChildren(branch.ChildA);
            var childBAverageLeafPosition = (1.0f - _growthModel.ChildDirectionRatio) * GetAverageLeafPositionOfChildren(branch.ChildB);

            return selfAverageLeafPosition + childAAverageLeafPosition + childBAverageLeafPosition;
        }

        public IEnumerable<Vector3> GetLocalLeafPositions()
        {
            if (!IsLeaf) yield break;
            if (_depth < _growthModel.MinDepthForLeafToAppear) yield break;

            //guaranteeing the leaf will be in the same position with the same seed per branch
            Random.InitState(_id);
            for (var i = 0; i < _growthModel.LeafCount; i++)
            {
                var offset = new Vector3(
                    Random.Range(-1f, 1f) * _growthModel.LeafSpread.x,
                    Random.Range(-1f, 1f) * _growthModel.LeafSpread.y,
                    Random.Range(-1f, 1f) * _growthModel.LeafSpread.z
                );

                yield return offset + Length * ToDirection;
            }
        }

        //todo: looks suspicious
        private static Vector3 GetNoiseVector() => new Vector3(Random.value % 100, Random.value % 100, Random.value % 100) / 100 - Vector3.one * 0.5f;
    }
}