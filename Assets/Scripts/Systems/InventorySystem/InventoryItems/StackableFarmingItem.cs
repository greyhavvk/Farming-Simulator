using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.InventorySystem
{
    public class StackableFarmingItem : FarmingItem
    {
        [SerializeField] private int maxStackCount;
        private int _currentStackCount=0;

        public int EmptyCount => maxStackCount - _currentStackCount;
        public int CurrentStackCount => _currentStackCount;

        public void SetStackCount(int stackCount)
        {
            _currentStackCount = Mathf.Clamp(stackCount,0,maxStackCount);
        }

    }
}