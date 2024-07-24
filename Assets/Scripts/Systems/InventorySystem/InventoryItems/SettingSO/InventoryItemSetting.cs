using Systems.FinanceSystem;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems.SettingSO
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "InventoryItem", order = 0)]
    public class InventoryItemSetting : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private ItemFinanceData financeData;
        [SerializeField] private int itemIndexID;
        public Sprite Icon => icon;
        public int ItemIndexID => itemIndexID;
        public ItemFinanceData FinanceData => financeData;
    }
}