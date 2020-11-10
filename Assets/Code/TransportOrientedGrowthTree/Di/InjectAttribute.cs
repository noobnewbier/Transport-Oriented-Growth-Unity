using System;
using JetBrains.Annotations;
using UnityEngine.Scripting;

namespace TransportOrientedGrowthTree.Di
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectAttribute : PreserveAttribute
    {
    }
}