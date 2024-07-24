using System;
using Systems.InventorySystem;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;

namespace Core.UIManager
{
    public class FarmingItemUI : InventoryItemUI
    {
        private Action<FarmingItemData, FarmingItemUI> _onFarmingItemUIPointerDown;
        private Action<FarmingItemUI> _onFarmingItemUIPointerUp;

        public void InitializeFarmingItemUI(Action<FarmingItemData, FarmingItemUI> onFarmingItemUIPointerDown,
            Action<FarmingItemUI> onFarmingItemUIPointerUp)
        {
            _onFarmingItemUIPointerDown = onFarmingItemUIPointerDown;
            _onFarmingItemUIPointerUp = onFarmingItemUIPointerUp;
        }

        //TODO buranın ayara ihtiyacı var.
        public override void SetVisual()
        {
            if (farmingItemData!=null)
            {
                itemImage.gameObject.SetActive(true);
                itemImage.sprite = farmingItemData.FarmingItem.Icon;
                itemImage.CrossFadeAlpha(1f, 0.05f, true);
            }
            else
            {
                itemImage.gameObject.SetActive(false);
            }
        }
        
        protected override void PointerUpTriggered()
        {
            _onFarmingItemUIPointerUp?.Invoke(this);
        }

        protected override void PointerDownTriggered()
        {
            _onFarmingItemUIPointerDown?.Invoke(farmingItemData, this);
        }
    }
}