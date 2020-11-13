using UnityEngine;

namespace TransportOrientedGrowthTree.Core
{
    public class TreeMonoBehaviour : MonoBehaviour
    {
        private Tree _tree;
        [SerializeField] private GrowthModelScriptable growthModelScriptable;
        [SerializeField] private float growthRate;

        public Tree Tree => _tree ?? (_tree = new Tree(growthModelScriptable.ToModel()));

        public void Grow()
        {
            Tree.Grow(growthRate);
        }
    }
}