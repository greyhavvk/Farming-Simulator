using System;
using Systems.InventorySystem;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;
using TMPro;
using UnityEngine;

namespace Core.UIManager
{
    public class FarmingItemUI : InventoryItemUI
    {
        [SerializeField] private TMP_Text stackText;
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
                if (farmingItemData.FarmingItem is StackableFarmingItem stackableFarmingItem)
                {
                    stackText.gameObject.SetActive(true);
                    stackText.text = stackableFarmingItem.CurrentStackCount.ToString();
                }
                else
                {
                    stackText.gameObject.SetActive(false);
                }
            }
            else
            {
                stackText.gameObject.SetActive(false);
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

        public void DisableListeners()
        {
            _onFarmingItemUIPointerDown = null;
            _onFarmingItemUIPointerUp = null;
        }
    }
}