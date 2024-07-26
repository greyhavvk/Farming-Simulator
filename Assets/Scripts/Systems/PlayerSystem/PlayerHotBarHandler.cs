using System;
using System.Linq;
using Core.UIManager;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.PlayerSystem
{
    public class PlayerHotBarHandler : MonoBehaviour
    {
        [SerializeField] private int hotBarSize;
        private FarmingItemData[] _itemDatas;
        private int _currentSelectedItemIndex;
        private IInventoryUI _inventoryUI;
        private Action<FarmingItemData> _onItemSelectedFromHotBar;

        public void Initialize(IInventoryUI inventoryUI, Action<FarmingItemData> onItemSelectedFromHotBar)
        {
            _onItemSelectedFromHotBar = onItemSelectedFromHotBar;
            _inventoryUI = inventoryUI;
            _itemDatas = new FarmingItemData[hotBarSize];
            _currentSelectedItemIndex = 0;

            _inventoryUI.InitializeHotBarUI(hotBarSize, ItemSelectedFromIndex);
            _inventoryUI.SetHotBarList(_itemDatas.ToList());
            ItemSelectedFromIndex(_currentSelectedItemIndex);
        }

        private void OnDisable()
        {
            _onItemSelectedFromHotBar = null;
        }

        public void RefreshHotBar()
        {
            _itemDatas = _inventoryUI.CalculateNewHotBarItemList().ToArray();
            _inventoryUI.SetHotBarList(_itemDatas.ToList());
            _inventoryUI.RefreshInventory();
            ItemSelectedFromIndex(_currentSelectedItemIndex);
        }

        public void RemoveItemFromHotBar(FarmingItemData itemData)
        {
            for (int i = 0; i < _itemDatas.Length; i++)
            {
                if (_itemDatas[i] == itemData)
                {
                    _itemDatas[i] = null;
                    _inventoryUI.SetHotBarList(_itemDatas.ToList());
                    break;
                }
            }
        }

        private void ItemSelectedFromIndex(int hotBarIndex)
        {
            _currentSelectedItemIndex = hotBarIndex;
            _inventoryUI.ItemSelectedFromHotBar(_currentSelectedItemIndex);
            _onItemSelectedFromHotBar?.Invoke(_itemDatas[_currentSelectedItemIndex]);
        }

        private void SelectItemFromIndexAdder(int indexAdder)
        {
            _currentSelectedItemIndex += indexAdder;
            if (_currentSelectedItemIndex < 0)
            {
                _currentSelectedItemIndex = hotBarSize - 1;
            }
            else if (_currentSelectedItemIndex >= hotBarSize)
            {
                _currentSelectedItemIndex = 0;
            }

            ItemSelectedFromIndex(_currentSelectedItemIndex);
        }


        public void HandleItemSelectScroll(float scrollDelta)
        {
            if (scrollDelta != 0)
            {
                int scrollDirection = scrollDelta > 0 ? 1 : -1;
                SelectItemFromIndexAdder(scrollDirection);
            }
        }
    }
}
