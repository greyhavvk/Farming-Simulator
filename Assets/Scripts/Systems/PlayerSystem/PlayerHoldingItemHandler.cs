using System;
using System.Collections.Generic;
using Systems.FarmingSystems;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.PlayerSystem
{
    public class PlayerHoldingItemHandler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> holdingItems;
        private FarmingItemData _holdingItem;
        private IInteractedField _interactedField;
        
        public void SetHoldingItem(FarmingItemData farmingItemData)
        {
            _holdingItem = farmingItemData;
            SetVisual();
        }

        private void SetVisual()
        {
            foreach (var holdingItem in holdingItems)
            {
               holdingItem.SetActive(false);
            }

            if (_holdingItem != null)
            {
                holdingItems[_holdingItem.FarmingItem.ItemIndexID].SetActive(true);
            }
        }

        private void UseItemAnimation()
        {
            //TODO eşya kullanma animasyonları ayarlanmalı
        }

        public void Initialize(IInteractedField interactedField)
        {
            _interactedField = interactedField;
            SetVisual();
        }

        public void FieldInteracted(GameObject field)
        {
            if (_holdingItem!=null)
            {
                _interactedField?.OnItemUseOnField(_holdingItem, field);
                UseItemAnimation();
            }
        }

        public void InteractingTriedAnimation()
        {
            //TODO boş el animasyonu yap.
        }
    }
}