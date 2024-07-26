using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UIManager.Inventory
{
    public class InventoryItemUI : InteractablePanel
    {
        [SerializeField] protected Image itemImage;
        protected FarmingItemData farmingItemData;

        protected override void OnClick(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.PointerDown:
                    PointerDownTriggered();
                    break;
                case ActionType.Drop or ActionType.PointerUp:
                    PointerUpTriggered();
                    break;
            }
        }

        protected virtual void PointerUpTriggered()
        {
            // Placeholder for derived class implementation
        }

        protected virtual void PointerDownTriggered()
        {
            // Placeholder for derived class implementation
        }

        public virtual void SetVisual()
        {
            // Placeholder for derived class implementation
        }
        
        public void FadeOut()
        {
            itemImage.CrossFadeAlpha(0.3f, 0.05f, true);
        }
        
        public FarmingItemData GetInventoryItemData()
        {
            return farmingItemData;
        }
        
        public void SetInventoryItemData(FarmingItemData farmingItemData)
        {
            this.farmingItemData = farmingItemData;
        }
    }
}