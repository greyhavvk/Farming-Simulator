namespace Systems.InventorySystem
{
    public class InventoryModel
    {
        private InventoryItemData[] _items;

        public InventoryModel(InventoryItemData[] items)
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
                if (itemData is FarmingItemData farmingItemData)
                {
                    if (CanStackItems(farmingItemData.FarmingItem, stackableFarmingItem))
                    {
                        FillItemStackFromAnother(farmingItemData, stackableFarmingItem.FarmingItemData);
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
                Seed existingSeed when newItem is Seed newSeed => existingSeed.PlantType == newSeed.PlantType,
                Harvest existingHarvest when newItem is Harvest newHarvest => existingHarvest.PlantType ==
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

        public void FillItemStackFromAnother(InventoryItemData targetItem, InventoryItemData fillerItem)
        {
            var targetStackableItem = (targetItem as FarmingItemData)?.FarmingItem as StackableFarmingItem;
            var fillerStackableItem =  (fillerItem as FarmingItemData)?.FarmingItem as StackableFarmingItem;

            if (targetStackableItem != null)
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

            if (fillerStackableItem != null)
            {
                fillerStackableItem.SetStackCount(fillerStackableItem.CurrentStackCount - addedCount);
            }
        }

        private bool IsEmpty(StackableFarmingItem stackableItem)
        {
            return stackableItem != null && stackableItem.CurrentStackCount == 0;
        }

        private void RemoveItemFromInventory(InventoryItemData itemData)
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
    }
}