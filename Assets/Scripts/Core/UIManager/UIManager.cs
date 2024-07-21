using System;
using System.Collections.Generic;
using Core.InputManager;
using Systems.FarmingSystems;
using Systems.InventorySystem;
using UnityEngine;

namespace Core.UIManager
{
    public class UIManager : MonoBehaviour, ITaskUI, IInventoryUI
    {
        [SerializeField] private PlantTaskUIPanel plantTaskUIPanel;
        [SerializeField] private GameObject moveTaskUIPanel;
        [SerializeField] private InventoryUIPanel inventoryUIPanel;

        public void Initialize(Action onRefreshInventoryRequested,Action<InventoryItemData> onDropItemFromInventory, Action<InventoryItemData, InventoryItemData> onFillStackFromAnother, IInventoryUIInput inventoryUIInput)
        {
            inventoryUIPanel.Initialize(onRefreshInventoryRequested, onDropItemFromInventory,onFillStackFromAnother, inventoryUIInput);
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

        public void SetInventoryList(List<InventoryItemData> itemDatas)
        {
            inventoryUIPanel.SetInventoryList(itemDatas);
        }

        public void SetHotBarList(List<InventoryItemData> hotBarList)
        {
            inventoryUIPanel.SetInventoryList(hotBarList);        }

        public List<InventoryItemData> CalculateNewHotBarItemList()
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

        public List<InventoryItemData> CalculateNewFarmingItemList()
        {
            return inventoryUIPanel.CalculateNewFarmingItemList();
        }
    }
}