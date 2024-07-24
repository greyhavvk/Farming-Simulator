using System;
using System.Collections.Generic;
using Systems.InventorySystem;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;

namespace Core.UIManager
{
    public interface IInventoryUI
    {
        void SetInventoryList(List<FarmingItemData> itemDatas);
        void SetHotBarList(List<FarmingItemData> hotBarList);
        void RefreshInventory();

        void CloseInventoryPanel();
        void OpenInventoryPanel();
        List<FarmingItemData> CalculateNewFarmingItemList();
        List<FarmingItemData> CalculateNewHotBarItemList();
        void InitializeInventoryUI(int inventorySize);
        void InitializeHotBarUI(int hotBarSize, Action<int> hotBarButtonClicked);
        void ItemSelectedFromHotBar(int currentSelectedItemIndex);
    }
}