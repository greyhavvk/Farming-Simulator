using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.FarmingSystems
{
    public interface IInteractedField
    {
        void OnItemUseOnField(FarmingItemData farmingItem, GameObject interactedField);
    }
}