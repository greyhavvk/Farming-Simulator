using Systems.FarmingSystems;
using Systems.FinanceSystem;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems
{
    public class HarvestProductFarmingItem : StackableFarmingItem
    {
        public PlantType PlantType { get; }

        public HarvestProductFarmingItem(FarmingItemData farmingItemData, ItemFinanceData itemFinanceData, int itemIndexID, Sprite icon, int maxStackCount, int initStackCount, PlantType plantType) : base(farmingItemData, itemFinanceData, itemIndexID, icon, maxStackCount,initStackCount)
        {
            PlantType = plantType;
        }
    }
}