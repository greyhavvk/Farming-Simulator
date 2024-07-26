using System;
using System.Collections.Generic;
using System.Linq;
using Core.InputManager;
using DG.Tweening;
using Systems.InventorySystem;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UIManager
{
    public class InventoryUIPanel : MonoBehaviour, IInventoryUI
    {
        [SerializeField] private FarmingItemUI refFarmingItemUI;
        [SerializeField] private HotBarItemUI refHotBarItemUI;
        [SerializeField] private List<FarmingItemUI> existingFarmingItemUIs;
        [SerializeField] private List<HotBarItemUI> existingHotBarItemUIs;
        [SerializeField] private DropItemContainerPanel dropItemContainerPanel;
        [SerializeField] private Transform farmingItemUIParent;
        [SerializeField] private Transform hotBarItemUIParent;
        [SerializeField] private GameObject inventoryUIPanel;
        [SerializeField] private Image holdingCursor;
        [SerializeField] private Image holdingItemImage;

        private FarmingItemData _holdingItemData;
        private FarmingItemUI _originOfHoldingData;
        private Vector2 _cursorInitialPos;
        private Vector3 _panelInitialScale;
        private Action _onRefreshRequested;
        private Action<FarmingItemData> _onItemDroppedInventory;
        private Action<FarmingItemData, FarmingItemData> _onFillStackFromAnother;
        private IInventoryUIInput _inventoryUIInput;

        private const float AnimationTime = .5f;

        public void Initialize(Action onRefreshInventoryRequested,Action<FarmingItemData> onItemDroppedInventory,Action<FarmingItemData, FarmingItemData> onFillStackFromAnother, IInventoryUIInput inventoryUIInput)
        {
            _onFillStackFromAnother = onFillStackFromAnother;
            _onItemDroppedInventory = onItemDroppedInventory;
            dropItemContainerPanel.Initialize(DropItemFromInventory);
            _inventoryUIInput = inventoryUIInput;
            _onRefreshRequested = onRefreshInventoryRequested;
            _panelInitialScale = inventoryUIPanel.transform.localScale;
            inventoryUIPanel.transform.localScale = Vector3.zero;
            _cursorInitialPos = holdingCursor.transform.position;
        }

        public void DisableListeners()
        {
            dropItemContainerPanel.DisableListeners();
            foreach (var existingFarmingItemUI in existingFarmingItemUIs)
            {
                existingFarmingItemUI.DisableListeners();
            }
            
            foreach (var existingHotBarItemUI in existingHotBarItemUIs)
            {
                existingHotBarItemUI.DisableListeners();
            }
        }

        private void Update()
        {
            if (inventoryUIPanel.activeSelf)
            {
                UpdateCursorPosition();
            }
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

        private void SetHoldingItem(FarmingItemData farmingItemData, FarmingItemUI panel)
        {
            _holdingItemData = farmingItemData;
            _originOfHoldingData = panel;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            bool itemDataIsEmpty = _holdingItemData != null;
            holdingItemImage.sprite = itemDataIsEmpty ? _holdingItemData.FarmingItem.Icon : null;
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
                if (farmingItemData.FarmingItem is SeedFarmingItem seed && holdingFarmingItemData.FarmingItem is SeedFarmingItem holdingSeed)
                {
                    if (seed.PlantType == holdingSeed.PlantType)
                    {
                        _onFillStackFromAnother?.Invoke(farmingItemData, holdingFarmingItemData);
                        ClearHoldingItem();
                        return;
                    }
                }
                else if (farmingItemData.FarmingItem is HarvestProductFarmingItem harvest && holdingFarmingItemData.FarmingItem is SeedFarmingItem holdingHarvest)
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
            if (panel.GetInventoryItemData()==_holdingItemData)
            {
                CancelPickUp();
            }
            else
            {
                var tempData = _holdingItemData;
                _originOfHoldingData.SetInventoryItemData(panel.GetInventoryItemData());
                panel.SetInventoryItemData(tempData);
            }
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

        public void SetInventoryList(List<FarmingItemData> itemDatas)
        {
            for (int i = 0; i < existingFarmingItemUIs.Count; i++)
            {
                var itemUI = existingFarmingItemUIs[i];
                itemUI.SetInventoryItemData(i < itemDatas.Count ? itemDatas[i] : null);
                itemUI.SetVisual();
            }
        }

        public void SetHotBarList(List<FarmingItemData> hotBarList)
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
                FarmingItemUI newFarmingItemUI = Instantiate(refFarmingItemUI, farmingItemUIParent);
                existingFarmingItemUIs.Add(newFarmingItemUI);
                newFarmingItemUI.InitializeFarmingItemUI(FarmingItemUIPointerDown, FarmingItemUIPointerUp);
            }
        }

        public void InitializeHotBarUI(int hotBarSize, Action<int> hotBarButtonClicked)
        {
            for (int i = 0; i < hotBarSize; i++)
            {
                GameObject newHotBarItem = Instantiate(refHotBarItemUI.gameObject, hotBarItemUIParent);
                var item = newHotBarItem.GetComponent<HotBarItemUI>();
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

        private void FarmingItemUIPointerDown(FarmingItemData farmingItemData, FarmingItemUI holdToFarmingItemUI)
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
            dropItemContainerPanel.gameObject.SetActive(false);
            SetHotBarsButtonActive(true);

        }

        public void OpenInventoryPanel()
        {
            inventoryUIPanel.SetActive(true);
            dropItemContainerPanel.gameObject.SetActive(true);
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

        public List<FarmingItemData> CalculateNewFarmingItemList()
        {
            return GetExistingFarmingItemUIs()
                .Select(t => t.GetInventoryItemData())
                .ToList();
        }
        
        public List<FarmingItemData> CalculateNewHotBarItemList()
        {
            return GetExistingHotBarItemUIs()
                .Select(t => t.GetInventoryItemData())
                .ToList();
        }
    }
}
