using Systems.FarmingSystems;
using Systems.FinanceSystem;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems
{
    public class FarmingTool : FarmingItem
    {
        public FarmingJobType FarmingJobType { get; }
        public float FarmingJobSpeedAdder { get; }
        
        public FarmingTool(FarmingItemData farmingItemData, ItemFinanceData itemFinanceData, int itemIndexID, Sprite icon, FarmingJobType farmingJobType, float farmingJobSpeedAdder) : base(farmingItemData, itemFinanceData, itemIndexID, icon)
        {
            FarmingJobType = farmingJobType;
            FarmingJobSpeedAdder = farmingJobSpeedAdder;
        }
    }
}