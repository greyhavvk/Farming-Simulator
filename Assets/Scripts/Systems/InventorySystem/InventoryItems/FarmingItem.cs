using Systems.FinanceSystem;
using UnityEngine;

namespace Systems.InventorySystem
{
    public class FarmingItem : MonoBehaviour, IFarmingItem
    {
        [SerializeField] private FarmingItemData farmingItemData;
        [SerializeField] private ItemFinanceData financeData;
        [SerializeField] private int dropItemIndex;
        public FarmingItemData FarmingItemData
        {
            get
            {
                farmingItemData.FarmingItem = this;
                return farmingItemData;
            }
        }

        public ItemFinanceData ItemFinanceData => financeData;
        public int DropItemIndex => dropItemIndex;
    }
}