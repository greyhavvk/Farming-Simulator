using System.Collections.Generic;
using Core.ObjectPool;
using Core.UIManager.Inventory;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;
using Systems.InventorySystem.InventoryItems.SettingSO;
using UnityEngine;

namespace Systems.InventorySystem
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private Transform dropCenterTransform;
        [SerializeField] private DropItem refDropItem;
        [SerializeField] private List<InventoryItemSetting> itemSettings;
        private ObjectPool _dropItemPool;
        [SerializeField] private int inventorySize;
        private IInventoryUI _inventoryUI;

        private InventoryModel _inventoryModel;
        private DropItemHandler _dropItemHandler;
        private bool _inventoryOpened;

        public void Initialize(IInventoryUI inventoryUI)
        {
            _dropItemPool = new ObjectPool(refDropItem, 10, new DropItemPoolableObjectInitializeData());
            var items = new FarmingItemData[inventorySize];
            _inventoryUI = inventoryUI;
            _inventoryUI.InitializeInventoryUI(inventorySize);

            _inventoryModel = new InventoryModel(items);
            _dropItemHandler = new DropItemHandler(_dropItemPool, dropCenterTransform);
        }

        public void InventoryUITriggered()
        {
            _inventoryOpened = !_inventoryOpened;
            if (_inventoryOpened)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
        
        private void OpenInventory()
        {
            UpdateInventoryUI();
            _inventoryUI.OpenInventoryPanel();
        }

        private void CloseInventory()
        {
            UpdateItemList();
            _inventoryUI.CloseInventoryPanel();
        }

        public void ReduceStackableItemFromSeedStack(StackableFarmingItem stackableFarmingItem, int reduceCount, out FarmingItemData removedItem)
        {
            _inventoryModel.ReduceStackableItemFromSeedStack(stackableFarmingItem, reduceCount, out removedItem);
            UpdateInventoryUI();
        }

        public void RefreshInventory()
        {
            UpdateItemList();
            _inventoryUI.RefreshInventory();
        }

        public void RemoveItem(FarmingItemData farmingItemData)
        {
            _inventoryModel.RemoveItemFromInventory(farmingItemData);
            UpdateInventoryUI();
        }

        public void TryAddDroppedItem(GameObject addedItemGameObject)
        {
            _dropItemHandler.TryAddItem(addedItemGameObject, _inventoryModel);
        }

        public void DropItemFromInventory(FarmingItemData itemData)
        {
            _dropItemHandler.DropItemFromInventory(itemData);
        }

        public void InstanceItemFromPosition(int itemID, Vector3 position)
        {
            var inventoryItemSetting = itemSettings.Find(match: setting => setting.ItemIndexID == itemID);
           
            if (inventoryItemSetting)
            {
                FarmingItem farmingItem;
                FarmingItemData farmingItemData=new FarmingItemData();
                switch (inventoryItemSetting)
                {
                    case HarvestProductFarmingItemSetting harvestProductFarmingItemSetting:
                        farmingItem = new HarvestProductFarmingItem(farmingItemData, inventoryItemSetting.FinanceData,
                            inventoryItemSetting.ItemIndexID, inventoryItemSetting.Icon,
                            harvestProductFarmingItemSetting.MaxStackCount,1, harvestProductFarmingItemSetting.PlantType);
                        break;
                    case SeedFarmingItemSetting seedFarmingItemSetting:
                        farmingItem = new SeedFarmingItem(farmingItemData, inventoryItemSetting.FinanceData,
                            inventoryItemSetting.ItemIndexID, inventoryItemSetting.Icon,
                            seedFarmingItemSetting.MaxStackCount,1, seedFarmingItemSetting.PlantType, seedFarmingItemSetting.FarmingJobs, seedFarmingItemSetting.ProductID);
                        break;
                    case FarmingToolItemSetting farmingToolItemSetting:
                        farmingItem = new FarmingTool(farmingItemData, inventoryItemSetting.FinanceData,
                            inventoryItemSetting.ItemIndexID, inventoryItemSetting.Icon, farmingToolItemSetting.FarmingJobType, farmingToolItemSetting.FarmingJobSpeedAdder);
                        break;
                    default:
                        farmingItem = new FarmingItem(farmingItemData, inventoryItemSetting.FinanceData,
                            inventoryItemSetting.ItemIndexID, inventoryItemSetting.Icon);
                        break;
                }
                farmingItemData.FarmingItem = farmingItem;

                _dropItemHandler.InstanceItemFromPosition(farmingItemData, position);
            }
        }

        private void UpdateItemList()
        {
            _inventoryModel.SetItemList(_inventoryUI.CalculateNewFarmingItemList().ToArray());
        }

        private void UpdateInventoryUI()
        {
            _inventoryUI.SetInventoryList(_inventoryModel.GetItemList());
        }

        public void FillItemStackFromAnother(FarmingItemData target, FarmingItemData filler)
        {
            _inventoryModel.FillItemStackFromAnother(target,filler);
        }

        public List<FarmingItemData> GetItemsForSell()
        {
            return _inventoryModel.GetItemsForSell();
        }
    }
}
