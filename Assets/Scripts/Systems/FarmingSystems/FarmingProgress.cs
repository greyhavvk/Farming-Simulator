using System;

namespace Systems.FarmingSystems
{
    [Serializable]
    public class FarmingProgress
    {
        public float JobCompletionRate => currentCompletionValue / targetCompletionValue;
        public float currentCompletionValue;
        public float targetCompletionValue;
        public float targetCompletionSpeedValue;
        public FarmingJobType farmingJobType;
    }
}