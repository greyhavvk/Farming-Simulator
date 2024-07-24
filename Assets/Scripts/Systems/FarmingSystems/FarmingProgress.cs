using System;
using UnityEngine;

namespace Systems.FarmingSystems
{
    [Serializable]
    public class FarmingProgress
    {
        public float JobCompletionRate => currentCompletionValue / targetCompletionValue;
        [HideInInspector]public float currentCompletionValue;
        public float targetCompletionValue;
        public float targetCompletionSpeedValue;
        public FarmingJobType farmingJobType;
    }
}