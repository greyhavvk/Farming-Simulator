using Systems.FinanceSystem;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems
{
    public class StackableFarmingItem : FarmingItem
    {
        private readonly int _maxStackCount;
        private int _currentStackCount=0;

        public int EmptyCount => _maxStackCount - _currentStackCount;
        public int CurrentStackCount => _currentStackCount;

        public void SetStackCount(int stackCount)
        {
            _currentStackCount = Mathf.Clamp(stackCount,0,_maxStackCount);
        }

        public StackableFarmingItem(FarmingItemData farmingItemData, ItemFinanceData itemFinanceData, 
            int itemIndexID, Sprite icon, int maxStackCount, int initStackCount) : base(farmingItemData, itemFinanceData, itemIndexID, icon)
        {
            _currentStackCount = 1;
            _maxStackCount = maxStackCount;
        }
    }
}