using UnityEngine;

namespace Systems.InventorySystem.InventoryItems.SettingSO
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "InventoryItem", order = 0)]
    public class StackableInventoryItemSetting:InventoryItemSetting
    {
        [SerializeField] private int maxStackCount;
        public int MaxStackCount => maxStackCount;
    }
}