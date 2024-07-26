using System;
using System.Collections.Generic;
using Core.InputManager;
using Core.UIManager.Finance;
using Core.UIManager.Inventory;
using Core.UIManager.Market;
using Core.UIManager.Task;
using Systems.FarmingSystems;
using Systems.InventorySystem.InventoryItems.Data;
using Systems.MarketSystem;
using UnityEngine;

namespace Core.UIManager
{
    public class UIManager : MonoBehaviour, ITaskUI, IInventoryUI, IFinanceUI,IMarketUI
    {
        [SerializeField] private PlantTaskUIPanel plantTaskUIPanel;
        [SerializeField] private GameObject moveTaskUIPanel;
        [SerializeField] private InventoryUIPanel inventoryUIPanel;
        [SerializeField] private FinanceUIPanel financeUIPanel;
        [SerializeField] private MarketUIPanelController marketUIPanelController;
        [SerializeField] private GameObject settingPanel;

        public void Initialize(Action onRefreshInventoryRequested, Action<FarmingItemData> onDropItemFromInventory,
            Action<FarmingItemData, FarmingItemData> onFillStackFromAnother, IInventoryUIInput inventoryUIInput)
        {
            inventoryUIPanel.Initialize(onRefreshInventoryRequested, onDropItemFromInventory, onFillStackFromAnother,
                inventoryUIInput);
            marketUIPanelController.Initialize();
        }

        public void DisableListeners()
        {
            inventoryUIPanel.DisableListeners();
        }

        public void UpdateMoveTaskUI()
        {
            moveTaskUIPanel.SetActive(true);
        }

        public void UpdateTaskUI(PlantType plantType, string taskMessage)
        {
            plantTaskUIPanel.gameObject.SetActive(true);
            plantTaskUIPanel.UpdateTaskUI(plantType,taskMessage);
        }

        public void ClearTaskUI()
        {
            plantTaskUIPanel.gameObject.SetActive(false);
            moveTaskUIPanel.SetActive(false);
        }

        public void SetInventoryList(List<FarmingItemData> itemDatas)
        {
            inventoryUIPanel.SetInventoryList(itemDatas);
        }

        public void SetHotBarList(List<FarmingItemData> hotBarList)
        {
            inventoryUIPanel.SetHotBarList(hotBarList);        }

        public List<FarmingItemData> CalculateNewHotBarItemList()
        {
            return inventoryUIPanel.CalculateNewHotBarItemList();
        }

        public void InitializeInventoryUI(int inventorySize)
        {
            inventoryUIPanel.InitializeInventoryUI(inventorySize);
        }

        public void InitializeHotBarUI(int hotBarSize, Action<int> hotBarButtonClicked)
        {
            inventoryUIPanel.InitializeHotBarUI(hotBarSize, hotBarButtonClicked);
        }

        public void ItemSelectedFromHotBar(int currentSelectedItemIndex)
        {
            inventoryUIPanel.ItemSelectedFromHotBar(currentSelectedItemIndex);
        }

        public void RefreshInventory()
        {
            inventoryUIPanel.RefreshInventory();
            
        }

        public void CloseInventoryPanel()
        {
            
            inventoryUIPanel.CloseInventoryPanel();
        }

        public void OpenInventoryPanel()
        {
           
            inventoryUIPanel.OpenInventoryPanel();
        }

        public List<FarmingItemData> CalculateNewFarmingItemList()
        {
            return inventoryUIPanel.CalculateNewFarmingItemList();
        }

        public void UpdateFinancePanelUI(int value)
        {
            financeUIPanel.UpdateFinancePanelUI(value);
        }

        public void UpdateMarketSellItemPanelUI(List<Product> products, Action<Product> onProductAction)
        {
            marketUIPanelController.UpdateMarketSellItemPanelUI(products,onProductAction);
        }

        public void UpdateMarketBuyItemPanelUI(List<Product> products, Action<Product> onProductAction)
        {
            marketUIPanelController.UpdateMarketBuyItemPanelUI(products,onProductAction);
        }

        public void UpdateMarketBuyBuildingPanelUI(List<Product> products, Action<Product> onProductAction)
        {
            marketUIPanelController.UpdateMarketBuyBuildingPanelUI(products, onProductAction);
        }

        public void OpenMarket()
        {
            
            marketUIPanelController.OpenMarket();
        }

        public void CloseMarket()
        {
           
            marketUIPanelController.CloseMarket();
        }

        public void OpenSettingPanel()
        {
            settingPanel.SetActive(true);
        }
        
        public void CloseSettingPanel()
        {
            settingPanel.SetActive(false);
        }
    }
}