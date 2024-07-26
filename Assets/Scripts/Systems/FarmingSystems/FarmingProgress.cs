using System;
using UnityEngine;

namespace Systems.FarmingSystems
{
    [Serializable]
    public class FarmingProgress
    {
        public float JobCompletionRate => currentCompletionValue / targetCompletionValue;
        public float currentCompletionValue=0;
        public float targetCompletionValue;
        public float targetCompletionSpeedValue;
        public FarmingJobType farmingJobType;
    }
}