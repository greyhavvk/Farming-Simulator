using System.Collections.Generic;
using System.Linq;
using Core.InputManager;
using Core.ObjectPool;
using Core.UIManager;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;
using Systems.InventorySystem.InventoryItems.SettingSO;
using UnityEngine;

namespace Systems.InventorySystem
{
    public class InventoryController : MonoBehaviour, IOpenClosedInventoryUI
    {
        [SerializeField] private Transform dropCenterTransform;
        [SerializeField] private DropItem refDropItem;
        [SerializeField] private List<InventoryItemSetting> itemSettings;
        private ObjectPool _dropItemPool;
        private FarmingItemData[] _items;
        [SerializeField] private int inventorySize;
        private IInventoryUI _inventoryUI;

        private InventoryModel _inventoryModel;
        private DropItemHandler _dropItemHandler;
        private bool _inventoryOpened;
        private IInventoryInput _inventoryInput;

        public void Initialize(IInventoryUI inventoryUI, IInventoryInput inventoryInput)
        {
            _inventoryInput = inventoryInput;
            _dropItemPool = new ObjectPool(refDropItem, 10, new DropItemPoolableObjectInitializeData());
            _items = new FarmingItemData[inventorySize];
            _inventoryUI = inventoryUI;
            _inventoryUI.InitializeInventoryUI(inventorySize);

            _inventoryModel = new InventoryModel(_items);
            _dropItemHandler = new DropItemHandler(_dropItemPool, dropCenterTransform);
        }
        
        private void Update()
        {
            if (_inventoryInput.GetInventoryUITriggerInput())
            {
                InventoryUITriggered();
            }
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

        public void ReduceStackableItemFromSeedStack(StackableFarmingItem stackableFarmingItem, int reduceCount)
        {
            _inventoryModel.ReduceStackableItemFromSeedStack(stackableFarmingItem, reduceCount);
        }

        public void RefreshInventory()
        {
            UpdateItemList();
            _inventoryUI.RefreshInventory();
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
                            harvestProductFarmingItemSetting.MaxStackCount, harvestProductFarmingItemSetting.PlantType);
                        break;
                    case SeedFarmingItemSetting seedFarmingItemSetting:
                        farmingItem = new SeedFarmingItem(farmingItemData, inventoryItemSetting.FinanceData,
                            inventoryItemSetting.ItemIndexID, inventoryItemSetting.Icon,
                            seedFarmingItemSetting.MaxStackCount, seedFarmingItemSetting.PlantType, seedFarmingItemSetting.FarmingJobs, seedFarmingItemSetting.ProductID);
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
            _items = _inventoryUI.CalculateNewFarmingItemList().ToArray();
        }

        private void UpdateInventoryUI()
        {
            _inventoryUI.SetInventoryList(_items.ToList());
        }

        public void FillItemStackFromAnother(FarmingItemData target, FarmingItemData filler)
        {
            _inventoryModel.FillItemStackFromAnother(target,filler);
        }
    }
}
