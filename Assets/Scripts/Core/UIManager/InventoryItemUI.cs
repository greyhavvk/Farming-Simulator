using Systems.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UIManager
{
    public class InventoryItemUI : InteractablePanel
    {
        [SerializeField] protected Image itemImage;
        protected InventoryItemData inventoryItemData;
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
        }

        protected virtual void PointerDownTriggered()
        {
        }

        public virtual void SetVisual()
        {
            
        }
        
        public void FadeOut()
        {
            itemImage.CrossFadeAlpha(0.3f, 0.05f, true);
        }
        
        public InventoryItemData GetInventoryItemData()
        {
            return inventoryItemData;
        }
        
        public void SetInventoryItemData(InventoryItemData farmingItemData)
        {
            inventoryItemData = farmingItemData;
        }
    }
}