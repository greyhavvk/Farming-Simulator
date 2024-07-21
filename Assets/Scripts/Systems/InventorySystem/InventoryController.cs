using System.Linq;
using Core.ObjectPool;
using Core.UIManager;
using UnityEngine;

namespace Systems.InventorySystem
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private Transform dropCenterTransform;
        [SerializeField] private DropItem refDropItem;
        private ObjectPool _dropItemPool;
        private InventoryItemData[] _items;
        [SerializeField] private int inventorySize;
        private IInventoryUI _inventoryUI;

        private InventoryModel _inventoryModel;
        private DropItemHandler _dropItemHandler;

        public void Initialize(IInventoryUI inventoryUI)
        {
            _dropItemPool = new ObjectPool(refDropItem, 10, new DropItemPoolableObjectInitializeData());
            _items = new InventoryItemData[inventorySize];
            _inventoryUI = inventoryUI;
            _inventoryUI.InitializeInventoryUI(inventorySize);

            _inventoryModel = new InventoryModel(_items);
            _dropItemHandler = new DropItemHandler(_dropItemPool, dropCenterTransform);
        }

        public void OpenInventory()
        {
            UpdateInventoryUI();
            _inventoryUI.OpenInventoryPanel();
        }

        public void CloseInventory()
        {
            UpdateItemList();
            _inventoryUI.CloseInventoryPanel();
        }

        public void RefreshInventory()
        {
            UpdateItemList();
            _inventoryUI.RefreshInventory();
        }

        public void TryAddItem(GameObject addedItemGameObject)
        {
            _dropItemHandler.TryAddItem(addedItemGameObject, _inventoryModel);
        }

        public void DropItemFromInventory(InventoryItemData itemData)
        {
            _dropItemHandler.DropItemFromInventory(itemData);
        }

        private void UpdateItemList()
        {
            _items = _inventoryUI.CalculateNewFarmingItemList().ToArray();
        }

        private void UpdateInventoryUI()
        {
            _inventoryUI.SetInventoryList(_items.ToList());
        }

        public void FillItemStackFromAnother(InventoryItemData target, InventoryItemData filler)
        {
            _inventoryModel.FillItemStackFromAnother(target,filler);
        }
    }
}
