using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TransportOrientedGrowthTree.Core
{
    public class Branch : IDisposable
    {
        private readonly GrowthModel _growthModel;

        private readonly int _id;
        private bool _isLeaf = true;

        private Branch _childA, _childB;
        private readonly Branch _parent;

        private readonly float _growthRatio;
        private readonly float _growthSpread;
        private readonly float _splitSize;
        private readonly int _depth;

        private readonly Vector3 _direction;
        private float _length, _radius, _crossSectionArea = 0.1f;


        public Branch(float growthRatio, float growthSpread, float splitSize, GrowthModel growthModel)
        {
            _growthModel = growthModel;
            _growthRatio = growthRatio;
            _growthSpread = growthSpread;
            _splitSize = splitSize;
            _direction = Vector3.up;
        }

        public Branch(Branch parent, int id, Vector3 direction)
        {
            _growthModel = parent._growthModel;
            _growthRatio = parent._growthRatio;
            _growthSpread = parent._growthSpread;
            _splitSize = parent._splitSize;
            _depth = parent._depth + 1;
            _parent = parent;
            _id = id;
            _direction = direction;
        }

        private float SplitRequirementOnLength => _splitSize * Mathf.Exp(-_growthModel.SplitDecay * _depth);

        public void Dispose()
        {
            _childA?.Dispose();
            _childB?.Dispose();
        }

        public void Grow(float feed)
        {
            _radius = Mathf.Sqrt(_crossSectionArea / Mathf.PI);

            if (_isLeaf)
                GrowAsLeaf(feed);
            else
                GrowAsParent(feed);
        }

        private void GrowAsParent(float feed)
        {
            var pass = GetPassRatio();

            _crossSectionArea += pass * feed / _length;
            feed *= 1 - pass;

            if (!HasEnoughFeedForChildren(feed)) return;

            _childA.Grow(feed * _growthModel.PassRatio);
            _childB.Grow(feed * (1 - _growthModel.PassRatio));
        }

        private static bool HasEnoughFeedForChildren(float feed) => feed < 1E-5;

        //eventually converge to 0.5 - conserving the cross-section area constraint
        private float GetPassRatio() =>
            (_childA._crossSectionArea + _childB._crossSectionArea) /
            (_childA._crossSectionArea + _childB._crossSectionArea + _crossSectionArea);

        private void GrowAsLeaf(float feed)
        {
            var growthInLength = Mathf.Pow(feed, 1f / 3f);

            _length += growthInLength;
            feed -= growthInLength * _crossSectionArea;

            _crossSectionArea += feed / _length; // todo: this look suspicious, can be replaced by our "newer formula"

            if (LongEnoughToSplit())
                Split();
        }

        private bool LongEnoughToSplit() => _length > SplitRequirementOnLength;

        private void Split()
        {
            _isLeaf = false;

            var (aDirection, bDirection) = GetDirectionsForChildren();
            _childA = new Branch(this, 2 * _id, aDirection);
            _childB = new Branch(this, 2 * _id + 1, bDirection);
        }

        private (Vector3 aDirection, Vector3 bDirection) GetDirectionsForChildren()
        {
            var directionWithHighestLeafDensity = GetDirectionWithHighestLeafDensity(_growthModel.LocalDepth);
            var normalToSelfDirectionAndHighestLeafDensityPlane = Vector3.Cross(_direction, directionWithHighestLeafDensity);
            var randomDirectionFlip = Random.value > 0.5 ? 1f : -1f;

            var aDirection = Vector3.LerpUnclamped(
                    randomDirectionFlip * _growthSpread * normalToSelfDirectionAndHighestLeafDensityPlane,
                    _direction,
                    _growthRatio
                )
                .normalized;
            var bDirection = Vector3.LerpUnclamped(
                    randomDirectionFlip * _growthSpread * -normalToSelfDirectionAndHighestLeafDensityPlane,
                    _direction,
                    1 - _growthRatio
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
                relativePositionToStartNode += ancestorNode._length * ancestorNode._direction;
                ancestorNode = ancestorNode._parent;
            }

            //Average relative to ancestor, shifted by rel ( + Noise )
            return _growthModel.Directedness * (GetAverageLeafPositionOfChildren(ancestorNode) - relativePositionToStartNode).normalized +
                   (1.0f - _growthModel.Directedness) * noiseVector;
        }

        private Vector3 GetAverageLeafPositionOfChildren(Branch b)
        {
            if (b._isLeaf) return b._length * b._direction;
            return b._length * b._direction + _radius * GetAverageLeafPositionOfChildren(b._childA) +
                   (1.0f - _radius) * GetAverageLeafPositionOfChildren(b._childB);
        }

        //todo: looks suspicious
        private static Vector3 GetNoiseVector() => new Vector3(Random.value % 100, Random.value % 100, Random.value % 100) / 100 - Vector3.one * 0.5f;
    }
}