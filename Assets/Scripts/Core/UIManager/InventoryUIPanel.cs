﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.InputManager;
using DG.Tweening;
using Systems.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UIManager
{
    public class InventoryUIPanel : MonoBehaviour, IInventoryUI
    {
        [SerializeField] private FarmingItemUI refFarmingItemUI;
        [SerializeField] private List<FarmingItemUI> existingFarmingItemUIs;
        [SerializeField] private List<HotBarItemUI> existingHotBarItemUIs;
        [SerializeField] private DropItemContainerPanel dropItemContainerPanel;
        [SerializeField] private Transform farmingItemUIParent;
        [SerializeField] private GameObject inventoryUIPanel;
        [SerializeField] private Image holdingCursor;
        [SerializeField] private Image holdingItemImage;

        private InventoryItemData _holdingItemData;
        private FarmingItemUI _originOfHoldingData;
        private Vector2 _cursorInitialPos;
        private Vector3 _panelInitialScale;
        private Action _onRefreshRequested;
        private Action<InventoryItemData> _onItemDroppedInventory;
        private Action<InventoryItemData, InventoryItemData> _onFillStackFromAnother;
        private IInventoryUIInput _inventoryUIInput;

        private const float AnimationTime = .5f;

        public void Initialize(Action onRefreshInventoryRequested,Action<InventoryItemData> onItemDroppedInventory,Action<InventoryItemData, InventoryItemData> onFillStackFromAnother, IInventoryUIInput inventoryUIInput)
        {
            _onFillStackFromAnother = onFillStackFromAnother;
            _onItemDroppedInventory = onItemDroppedInventory;
            dropItemContainerPanel.Initialize(DropItemFromInventory);
            _inventoryUIInput = inventoryUIInput;
            _onRefreshRequested = onRefreshInventoryRequested;
            _panelInitialScale = inventoryUIPanel.transform.localScale;
            inventoryUIPanel.transform.localScale = Vector3.zero;
            Cursor.lockState = CursorLockMode.Locked;
            _cursorInitialPos = holdingCursor.transform.position;
        }

        private void Update()
        {
            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                holdingCursor.transform.position = _cursorInitialPos;
            }
            else
            {
                holdingCursor.transform.position = _inventoryUIInput.GetScreenFollowerPosition();
                holdingCursor.enabled = _holdingItemData == null;
            }
        }

        private void SetHoldingItem(InventoryItemData farmingItemData, FarmingItemUI panel)
        {
            _holdingItemData = farmingItemData;
            _originOfHoldingData = panel;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            bool itemDataIsEmpty = _holdingItemData != null;
            holdingItemImage.sprite = itemDataIsEmpty ? _holdingItemData.Icon : null;
            holdingItemImage.gameObject.SetActive(itemDataIsEmpty);
        }

        private bool IsThereAnyHoldingItem() => _holdingItemData != null;

        private void HandleItemPlacement(FarmingItemUI panel)
        {
            if (panel.GetInventoryItemData() == null || panel == _originOfHoldingData)
            {
                PlaceItem(panel);
                ClearHoldingItem();
                return;
            }

            if (panel.GetInventoryItemData() is FarmingItemData farmingItemData && _holdingItemData is FarmingItemData holdingFarmingItemData)
            {
                if (farmingItemData.FarmingItem is Seed seed && holdingFarmingItemData.FarmingItem is Seed holdingSeed)
                {
                    if (seed.PlantType == holdingSeed.PlantType)
                    {
                        _onFillStackFromAnother?.Invoke(farmingItemData, holdingFarmingItemData);
                        ClearHoldingItem();
                        return;
                    }
                }
                else if (farmingItemData.FarmingItem is Harvest harvest && holdingFarmingItemData.FarmingItem is Seed holdingHarvest)
                {
                    if (harvest.PlantType == holdingHarvest.PlantType)
                    {
                        _onFillStackFromAnother?.Invoke(farmingItemData, holdingFarmingItemData);
                        ClearHoldingItem();
                        return;
                    }
                }
            }

            SwapItems(panel);
            ClearHoldingItem();
        }

        
        private void SwapItems(FarmingItemUI panel)
        {
            
            var tempData = _holdingItemData;
            _originOfHoldingData.SetInventoryItemData(panel.GetInventoryItemData());
            panel.SetInventoryItemData(tempData);
        }

        private void PlaceItem(FarmingItemUI panel)
        {
            if (_originOfHoldingData != null)
            {
                _originOfHoldingData.SetInventoryItemData(null);
            }
            panel.SetInventoryItemData(_holdingItemData);
        }

        private void ClearHoldingItem()
        {
            _holdingItemData = null;
            _originOfHoldingData = null;
            UpdateVisual();
            _onRefreshRequested?.Invoke();
        }

        private void DropItemFromInventory()
        {
             if (IsThereAnyHoldingItem())
             { 
                 _onItemDroppedInventory.Invoke(_originOfHoldingData.GetInventoryItemData());
                 _originOfHoldingData.SetInventoryItemData(null);
                 ClearHoldingItem();
             }
        }

        private void CancelPickUp()
        {
            if (_holdingItemData == null) return;

            if (_originOfHoldingData != null)
            {
                _originOfHoldingData.SetInventoryItemData(_holdingItemData);
            }

            ClearHoldingItem();
        }

        public void SetInventoryList(List<InventoryItemData> itemDatas)
        {
            for (int i = 0; i < existingFarmingItemUIs.Count; i++)
            {
                var itemUI = existingFarmingItemUIs[i];
                itemUI.SetInventoryItemData(i < itemDatas.Count ? itemDatas[i] : null);
                itemUI.SetVisual();
            }
        }

        public void SetHotBarList(List<InventoryItemData> hotBarList)
        {
            for (int i = 0; i < existingHotBarItemUIs.Count; i++)
            {
                var itemUI = existingHotBarItemUIs[i];
                itemUI.SetInventoryItemData(i < hotBarList.Count ? hotBarList[i] : null);
                itemUI.SetVisual();
            }
        }

        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                GameObject newFarmingItemUI = Instantiate(refFarmingItemUI.gameObject, farmingItemUIParent);
                var item = newFarmingItemUI.GetComponent<FarmingItemUI>();
                existingFarmingItemUIs.Add(item);
                item.InitializeFarmingItemUI(FarmingItemUIPointerDown, FarmingItemUIPointerUp);
            }
        }

        public void InitializeHotBarUI(int hotBarSize, Action<int> hotBarButtonClicked)
        {
            for (int i = 0; i < hotBarSize; i++)
            {
                GameObject newFarmingItemUI = Instantiate(refFarmingItemUI.gameObject, farmingItemUIParent);
                var item = newFarmingItemUI.GetComponent<HotBarItemUI>();
                existingHotBarItemUIs.Add(item);
                item.InitializeHotBarItemUI(FarmingItemPlacedToHotBar,hotBarButtonClicked, i);
            }
        }

        public void ItemSelectedFromHotBar(int currentSelectedItemIndex)
        {
            for (int i = 0; i < existingHotBarItemUIs.Count; i++)
            {
                existingHotBarItemUIs[i].SetSelectedVisual(i == currentSelectedItemIndex);
            }
        }

        private void FarmingItemUIPointerDown(InventoryItemData farmingItemData, FarmingItemUI holdToFarmingItemUI)
        {
            if (!IsThereAnyHoldingItem())
            {
                SetHoldingItem(farmingItemData, holdToFarmingItemUI);
                holdToFarmingItemUI.FadeOut();
            }
        }
        
        private void FarmingItemPlacedToHotBar(HotBarItemUI itemToPlacement)
        {
            itemToPlacement.SetInventoryItemData(_holdingItemData);
            if (_originOfHoldingData != null)
            {
                _originOfHoldingData.SetInventoryItemData(_holdingItemData);
            }
            ClearHoldingItem();
        }

        private void FarmingItemUIPointerUp(FarmingItemUI itemToPlacement)
        {
            if (IsThereAnyHoldingItem())
            {
                HandleItemPlacement(itemToPlacement);
            }
        }

        public void RefreshInventory()
        {
            foreach (var itemUI in existingFarmingItemUIs)
            {
                itemUI.SetVisual();
            }
            foreach (var itemUI in existingHotBarItemUIs)
            {
                itemUI.SetVisual();
            }
        }

        public void CloseInventoryPanel()
        {
            CancelPickUp();
            AnimatePanelScale(Vector3.zero, () => inventoryUIPanel.SetActive(false));
            SetHotBarsButtonActive(true);

        }

        public void OpenInventoryPanel()
        {
            inventoryUIPanel.SetActive(true);
            AnimatePanelScale(_panelInitialScale, null);
            SetHotBarsButtonActive(false);
        }

        private void SetHotBarsButtonActive(bool active)
        {
            foreach (var existingHotBarItemU in existingHotBarItemUIs)
            {
                existingHotBarItemU.SetButtonActive(active);
            }
        }

        private void AnimatePanelScale(Vector3 targetScale, TweenCallback onComplete)
        {
            DOTween.Kill(gameObject.name);
            inventoryUIPanel.transform.DOScale(targetScale, AnimationTime).SetId(gameObject.name).OnComplete(onComplete);
        }

        private IEnumerable<FarmingItemUI> GetExistingFarmingItemUIs() => existingFarmingItemUIs;
        private IEnumerable<HotBarItemUI> GetExistingHotBarItemUIs() => existingHotBarItemUIs;

        public List<InventoryItemData> CalculateNewFarmingItemList()
        {
            return GetExistingFarmingItemUIs()
                .Select(t => t.GetInventoryItemData())
                .ToList();
        }
        
        public List<InventoryItemData> CalculateNewHotBarItemList()
        {
            return GetExistingHotBarItemUIs()
                .Select(t => t.GetInventoryItemData())
                .ToList();
        }
    }
}
