using UnityEngine;

namespace Systems.InventorySystem.InventoryItems.SettingSO
{
    [CreateAssetMenu(fileName = "StackableInventoryItemSetting", menuName = "InventoryItem/StackableInventoryItemSetting", order = 0)]
    public class StackableInventoryItemSetting:InventoryItemSetting
    {
        [SerializeField] private int maxStackCount;
        public int MaxStackCount => maxStackCount;
    }
}