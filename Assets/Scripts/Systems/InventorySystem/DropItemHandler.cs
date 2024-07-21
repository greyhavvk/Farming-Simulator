using Core.ObjectPool;
using UnityEngine;

namespace Systems.InventorySystem
{
    public class DropItemHandler
    {
        private ObjectPool _dropItemPool;
        private Transform _dropCenterTransform;

        public DropItemHandler(ObjectPool dropItemPool, Transform dropCenterTransform)
        {
            _dropItemPool = dropItemPool;
            _dropCenterTransform = dropCenterTransform;
        }

        public void TryAddItem(GameObject addedItemGameObject, InventoryModel itemHandler)
        {
            var dropItem = addedItemGameObject.GetComponent<DropItem>();
            if (dropItem == null)
            {
                return;
            }

            bool collected = itemHandler.AddItem(dropItem.FarmingItem);
            if (collected)
            {
                dropItem.Collect();
            }
            else
            {
                dropItem.UpdateFarmingItem(dropItem.FarmingItem);
            }
        }

        public void DropItemFromInventory(InventoryItemData itemData)
        {
            var dropItem = _dropItemPool.GetEntity() as DropItem;
            if (dropItem == null) return;
            dropItem.UpdateFarmingItem((itemData as FarmingItemData)?.FarmingItem);
            dropItem.Drop(_dropCenterTransform.position);
        }
    }
}