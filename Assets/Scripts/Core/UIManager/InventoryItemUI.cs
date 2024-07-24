using Systems.InventorySystem;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UIManager
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