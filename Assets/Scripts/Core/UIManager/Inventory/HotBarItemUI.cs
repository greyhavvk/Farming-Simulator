using System;
using Systems.InventorySystem.InventoryItems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UIManager
{
    public class HotBarItemUI : InventoryItemUI
    {
        [SerializeField] private Button button;
        [SerializeField] private GameObject selectedVisual;
        [SerializeField] private TMP_Text stackText;
        private Action<HotBarItemUI> _onFarmingItemPlacedToHotBar;
        private Action<int> _hotBarButtonClicked;
        private int _index;
        public void InitializeHotBarItemUI(Action<HotBarItemUI> onFarmingItemPlacedToHotBar, Action<int> hotBarButtonClicked, int index)
        {
            _index = index;
            _hotBarButtonClicked = hotBarButtonClicked;
            _onFarmingItemPlacedToHotBar = onFarmingItemPlacedToHotBar;
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
            _onFarmingItemPlacedToHotBar?.Invoke(this);
        }

        protected override void PointerDownTriggered()
        {
            _onFarmingItemPlacedToHotBar?.Invoke(this);
        }

        public void OnHotBarButtonClicked()
        {
            if (button.isActiveAndEnabled)
            {
                _hotBarButtonClicked.Invoke(_index);
            }
        }

        public void SetButtonActive(bool active)
        {
            button.enabled = active;
        }

        public void SetSelectedVisual(bool selected)
        {
            selectedVisual.SetActive(selected);
        }

        public void DisableListeners()
        {
            _hotBarButtonClicked = null;
            _onFarmingItemPlacedToHotBar = null;
        }
    }
}