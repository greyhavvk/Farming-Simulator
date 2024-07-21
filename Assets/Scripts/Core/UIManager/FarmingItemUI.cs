using System;
using Systems.InventorySystem;

namespace Core.UIManager
{
    public class FarmingItemUI : InventoryItemUI
    {
        private Action<InventoryItemData, FarmingItemUI> _onFarmingItemUIPointerDown;
        private Action<FarmingItemUI> _onFarmingItemUIPointerUp;

        public void InitializeFarmingItemUI(Action<InventoryItemData, FarmingItemUI> onFarmingItemUIPointerDown,
            Action<FarmingItemUI> onFarmingItemUIPointerUp)
        {
            _onFarmingItemUIPointerDown = onFarmingItemUIPointerDown;
            _onFarmingItemUIPointerUp = onFarmingItemUIPointerUp;
        }

        //TODO buranın ayara ihtiyacı var.
        public override void SetVisual()
        {
            if (inventoryItemData!=null)
            {
                itemImage.gameObject.SetActive(true);
                itemImage.sprite = inventoryItemData.Icon;
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
            _onFarmingItemUIPointerDown?.Invoke(inventoryItemData, this);
        }
    }
}