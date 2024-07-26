using System.Collections.Generic;
using System.Linq;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;

namespace Systems.InventorySystem
{
    public class InventoryModel
    {
        private FarmingItemData[] _items;

        public InventoryModel(FarmingItemData[] items)
        {
            _items = items;
        }

        public bool AddItem(IFarmingItem addedItem)
        {
            if (addedItem is StackableFarmingItem stackableFarmingItem)
            {
                return AddStackableItem(stackableFarmingItem);
            }
            else
            {
                return AddNonStackableItem(addedItem);
            }
        }

        private bool AddStackableItem(StackableFarmingItem stackableFarmingItem)
        {
            if (TryAddToExistingStack(stackableFarmingItem)) return true;
            return TryAddToEmptySlot(stackableFarmingItem);
        }

        private bool TryAddToExistingStack(StackableFarmingItem stackableFarmingItem)
        {
            foreach (var itemData in _items)
            {
                if (itemData!=null)
                {
                    if (CanStackItems(itemData.FarmingItem, stackableFarmingItem))
                    {
                        FillItemStackFromAnother(itemData, stackableFarmingItem.FarmingItemData);
                        if (IsEmpty(stackableFarmingItem)) return true;
                    }
                }
            }

            return false;
        }

        private bool CanStackItems(IFarmingItem existingItem, StackableFarmingItem newItem)
        {
            return existingItem switch
            {
                SeedFarmingItem existingSeed when newItem is SeedFarmingItem newSeed => existingSeed.PlantType == newSeed.PlantType,
                HarvestProductFarmingItem existingHarvest when newItem is HarvestProductFarmingItem newHarvest => existingHarvest.PlantType ==
                                                                              newHarvest.PlantType,
                _ => false
            };
        }

        private bool TryAddToEmptySlot(StackableFarmingItem stackableFarmingItem)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null)
                {
                    _items[i] = stackableFarmingItem.FarmingItemData;
                    return true;
                }
            }

            return false;
        }

        private bool AddNonStackableItem(IFarmingItem addedItem)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null)
                {
                    _items[i] = addedItem.FarmingItemData;
                    return true;
                }
            }

            return false;
        }

        public void ReduceStackableItemFromSeedStack(StackableFarmingItem stackableFarmingItem, int reduceCount,out FarmingItemData removedItem)
        {
            stackableFarmingItem.SetStackCount(stackableFarmingItem.CurrentStackCount-reduceCount);
            removedItem = null;
            if (IsEmpty(stackableFarmingItem))
            {
                removedItem = stackableFarmingItem.FarmingItemData;
                RemoveItemFromInventory(stackableFarmingItem.FarmingItemData);
            }
            
        }

        public void FillItemStackFromAnother(FarmingItemData targetItem, FarmingItemData fillerItem)
        {
            var fillerStackableItem =  fillerItem?.FarmingItem as StackableFarmingItem;

            if (targetItem?.FarmingItem is StackableFarmingItem targetStackableItem)
            {
                TransferStack(targetStackableItem, fillerStackableItem);
            }

            if (IsEmpty(fillerStackableItem))
            {
                RemoveItemFromInventory(fillerItem);
            }
        }

        private void TransferStack(StackableFarmingItem targetStackableItem, StackableFarmingItem fillerStackableItem)
        {
            int emptyCount = targetStackableItem.EmptyCount;
            int addedCount = fillerStackableItem != null && fillerStackableItem.CurrentStackCount <= emptyCount
                ? fillerStackableItem.CurrentStackCount
                : emptyCount;

            targetStackableItem.SetStackCount(targetStackableItem.CurrentStackCount + addedCount);

            fillerStackableItem?.SetStackCount(fillerStackableItem.CurrentStackCount - addedCount);
        }

        private bool IsEmpty(StackableFarmingItem stackableItem)
        {
            return stackableItem is { CurrentStackCount: 0 };
        }

        public void RemoveItemFromInventory(FarmingItemData itemData)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == itemData)
                {
                    _items[i] = null;
                    break;
                }
            }
        }

        public List<FarmingItemData> GetItemList()
        {
            return _items.ToList();
        }

        public void SetItemList(FarmingItemData[] items)
        {
            _items = items;
        }

        public List<FarmingItemData> GetItemsForSell()
        {
            var newList = new List<FarmingItemData>();

            foreach (var item in _items)
            {
                if (item!=null)
                {
                    newList.Add(item);
                }
            }

            return newList;

        }
    }
}