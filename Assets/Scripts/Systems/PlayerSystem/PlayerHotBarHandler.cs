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
        private int _scrollCounter;
        private const int ScrollThreshold = 5; // Her 5 kaydırma hareketinde bir tetikleme

        public void Initialize(IInventoryUI inventoryUI, Action<FarmingItemData> onItemSelectedFromHotBar)
        {
            _onItemSelectedFromHotBar = onItemSelectedFromHotBar;
            _inventoryUI = inventoryUI;
            _itemDatas = new FarmingItemData[hotBarSize];
            _currentSelectedItemIndex = 0;
            _scrollCounter = 0;

            _inventoryUI.InitializeHotBarUI(hotBarSize, ItemSelectedFromHotBar);
            _inventoryUI.SetHotBarList(_itemDatas.ToList());
            ItemSelectedFromHotBar(_currentSelectedItemIndex);
        }

        public void RefreshHotBar()
        {
            _itemDatas = _inventoryUI.CalculateNewHotBarItemList().ToArray();
            _inventoryUI.SetHotBarList(_itemDatas.ToList());
            _inventoryUI.RefreshInventory();
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

        private void ItemSelectedFromHotBar(int hotBarIndex)
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
            ItemSelectedFromHotBar(_currentSelectedItemIndex);
        }


        public void HandleItemSelectScroll(float scrollDelta)
        {
            if (scrollDelta != 0)
            {
                _scrollCounter += (int)scrollDelta;

                if (Mathf.Abs(_scrollCounter) >= ScrollThreshold)
                {
                    int scrollDirection = _scrollCounter > 0 ? 1 : -1;
                    SelectItemFromIndexAdder(scrollDirection);
                    _scrollCounter = 0; // Sayaç sıfırlanır
                }
            }
            else
            {
                _scrollCounter = 0; 
            }
        }
    }
}
