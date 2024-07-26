using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems.SettingSO
{
    [CreateAssetMenu(fileName = "FarmingToolItemSetting", menuName = "InventoryItem/FarmingToolItemSetting", order = 0)]
    public class FarmingToolItemSetting:InventoryItemSetting
    {
        [SerializeField] private FarmingJobType farmingJobType;
        [SerializeField] private float farmingJobSpeedAdder;
        public FarmingJobType FarmingJobType => farmingJobType;
        public float FarmingJobSpeedAdder => farmingJobSpeedAdder;
    }
}