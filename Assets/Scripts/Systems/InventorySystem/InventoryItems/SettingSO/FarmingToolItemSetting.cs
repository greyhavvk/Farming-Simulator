using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems.SettingSO
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "InventoryItem", order = 0)]
    public class FarmingToolItemSetting:InventoryItemSetting
    {
        [SerializeField] private FarmingJobType farmingJobType;
        [SerializeField] private int farmingJobSpeedAdder;
        public FarmingJobType FarmingJobType => farmingJobType;
        public int FarmingJobSpeedAdder => farmingJobSpeedAdder;
    }
}