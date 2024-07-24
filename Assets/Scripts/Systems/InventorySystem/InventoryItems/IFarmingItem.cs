using Systems.FinanceSystem;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems
{
    public interface IFarmingItem
    {
        FarmingItemData FarmingItemData { get; }
        ItemFinanceData ItemFinanceData { get; }
        int ItemIndexID { get;}
        Sprite Icon { get;}
    }
}