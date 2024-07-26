using Core.ObjectPool;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.InventorySystem
{
    public class DropItemHandler
    {
        private readonly ObjectPool _dropItemPool;
        private readonly Transform _dropCenterTransform;

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

        public void DropItemFromInventory(FarmingItemData itemData)
        {
            if (GetDropItem(itemData, out var dropItem)) return;
            dropItem.Drop(_dropCenterTransform.position);
        }

        private bool GetDropItem(FarmingItemData itemData, out DropItem dropItem)
        {
            dropItem = _dropItemPool.GetEntity() as DropItem;
            if (dropItem == null) return true;
            dropItem.UpdateFarmingItem((itemData)?.FarmingItem);
            return false;
        }

        public void InstanceItemFromPosition(FarmingItemData farmingItemData, Vector3 position)
        {
            if (GetDropItem(farmingItemData, out var dropItem)) return;
            dropItem.Spawn(position);
        }
    }
}