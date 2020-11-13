using UnityEngine;
using UnityUtils.Constants;

namespace TransportOrientedGrowthTree.Core
{
    [CreateAssetMenu(fileName = nameof(GrowthModel), menuName = MenuName.Data + nameof(GrowthModel))]
    public class GrowthModelScriptable : ScriptableObject
    {
        [SerializeField] private float branchSpread = 0.45f;
        [SerializeField] private int childDirectionAccuracyInDepth = 2;
        [SerializeField] private float childDirectionRatio = 0.6f;
        [SerializeField] private float directedness = 0.5f;
        [SerializeField] private float minLengthToSplit = 2.5f;
        [SerializeField] private float nutritionRatio = 0.3f;
        [SerializeField] private float splitDecay = 1E-2f;

        public GrowthModel ToModel() =>
            new GrowthModel(
                nutritionRatio,
                splitDecay,
                directedness,
                childDirectionAccuracyInDepth,
                childDirectionRatio,
                branchSpread,
                minLengthToSplit
            );
    }
}