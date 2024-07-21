using Systems.FinanceSystem;
using UnityEngine;

namespace Systems.InventorySystem
{
    public interface IFarmingItem
    {
        FarmingItemData FarmingItemData { get; }
        ItemFinanceData ItemFinanceData { get; }
        int DropItemIndex { get;}
    }
}