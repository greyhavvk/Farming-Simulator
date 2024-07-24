using Systems.FarmingSystems;
using Systems.FinanceSystem;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems
{
    public class FarmingTool : FarmingItem
    {
        public FarmingJobType FarmingJobType { get; }
        public int FarmingJobSpeedAdder { get; }
        
        public FarmingTool(FarmingItemData farmingItemData, ItemFinanceData itemFinanceData, int itemIndexID, Sprite icon, FarmingJobType farmingJobType, int farmingJobSpeedAdder) : base(farmingItemData, itemFinanceData, itemIndexID, icon)
        {
            FarmingJobType = farmingJobType;
            FarmingJobSpeedAdder = farmingJobSpeedAdder;
        }
    }
}