using Systems.FinanceSystem;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems
{
    public class FarmingItem : IFarmingItem
    {
        public FarmingItem(FarmingItemData farmingItemData, ItemFinanceData itemFinanceData, int itemIndexID, Sprite icon)
        {
            FarmingItemData = farmingItemData;
            ItemFinanceData = itemFinanceData;
            ItemIndexID = itemIndexID;
            Icon = icon;
        }

        public FarmingItemData FarmingItemData { get; }
        public ItemFinanceData ItemFinanceData { get; }
        public int ItemIndexID { get; }
        public Sprite Icon { get; }
        
        
    }
}