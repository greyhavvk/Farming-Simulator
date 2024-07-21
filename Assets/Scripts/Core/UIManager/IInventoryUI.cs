using System;
using System.Collections.Generic;
using Systems.InventorySystem;

namespace Core.UIManager
{
    public interface IInventoryUI
    {
        void SetInventoryList(List<InventoryItemData> itemDatas);
        void SetHotBarList(List<InventoryItemData> hotBarList);
        void RefreshInventory();

        void CloseInventoryPanel();
        void OpenInventoryPanel();
        List<InventoryItemData> CalculateNewFarmingItemList();
        List<InventoryItemData> CalculateNewHotBarItemList();
        void InitializeInventoryUI(int inventorySize);
        void InitializeHotBarUI(int hotBarSize, Action<int> hotBarButtonClicked);
        void ItemSelectedFromHotBar(int currentSelectedItemIndex);
    }
}