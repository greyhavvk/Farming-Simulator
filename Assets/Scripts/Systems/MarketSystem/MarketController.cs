using System;
using System.Collections.Generic;
using System.Linq;
using Core.UIManager.Market;
using Systems.FinanceSystem;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;
using Systems.TaskSystem;
using UnityEngine;

namespace Systems.MarketSystem
{
    public class MarketController : MonoBehaviour
    {
        [SerializeField] private List<Product> itemsForSale;
        [SerializeField] private List<Product> buildingsForSale;
        [SerializeField] private Transform marketSpawnPoint;

        private Action<FarmingItemData> _onItemSold;
        private Action<int> _onBuildingBought;
        private Action<int, Vector3> _onItemBought;
        private IFinance _finance;
        private IMarketUI _marketUI;
        private Action<TaskData> _onUpdateTaskProgress;

        private Dictionary<Product, FarmingItemData> _itemsForSell;

        public void Initialize(IFinance finance, IMarketUI marketUI,Action<int> onBuildingBought,Action<int, Vector3> onItemBought, Action<FarmingItemData> onItemSold)
        {
            _onItemSold = onItemSold;
            _onBuildingBought = onBuildingBought;
            _onItemBought = onItemBought;
            _marketUI = marketUI;
            _finance = finance;
            UpdateMarketBuyItemPanelUI(itemsForSale, OnBuyItem);
            UpdateMarketBuyBuildingPanelUI(buildingsForSale, OnBuyBuilding);
            _itemsForSell = new Dictionary<Product, FarmingItemData>();
        }

        public void DisableListeners()
        {
            _onItemSold = null;
            _onBuildingBought = null;
            _onItemBought = null;
            _onUpdateTaskProgress = null;
        }

        public void MoneyValueChanged()
        {
            if (_finance!=null)
            {
                UpdateMarketBuyItemPanelUI(itemsForSale, OnBuyItem);
                UpdateMarketBuyBuildingPanelUI(buildingsForSale, OnBuyBuilding);
                if (_itemsForSell.Count>0)
                {
                    UpdateMarketSellItemPanelUI(_itemsForSell.Keys.ToList(), OnSellProduct);
                }
            }
        }

        public void OpenMarket(List<FarmingItemData> itemsForSell)
        {
            _itemsForSell= SetItemsForSell(itemsForSell);
            UpdateMarketSellItemPanelUI(_itemsForSell.Keys.ToList(), OnSellProduct);
            UpdateMarketBuyItemPanelUI(itemsForSale, OnBuyItem);
            UpdateMarketBuyBuildingPanelUI(buildingsForSale, OnBuyBuilding);
            _marketUI.OpenMarket();
        }

        public void CloseMarket()
        {
            _marketUI.CloseMarket();
            _itemsForSell.Clear();
        }

        private Dictionary<Product,FarmingItemData> SetItemsForSell(List<FarmingItemData> itemsForSell)
        {
            var newList = new Dictionary<Product,FarmingItemData>();
            foreach (var item in itemsForSell)
            {
                if (item==null)
                {
                    continue;
                }

                int prize;
                int stack = 0;
                if ( item.FarmingItem is StackableFarmingItem stackableFarmingItem)
                {
                    prize = stackableFarmingItem.CurrentStackCount * item.FarmingItem.ItemFinanceData.sellValue;
                    stack = stackableFarmingItem.CurrentStackCount;
                }
                else
                {
                    prize=item.FarmingItem.ItemFinanceData.sellValue;
                }

                var product=new Product()
                {
                    itemFinanceData = item.FarmingItem.ItemFinanceData,
                    productSprite = item.FarmingItem.Icon,
                    Prize =prize,
                    canEnoughMoney = true,
                    stack = stack,
                };
                newList.Add(product, item);
            }

            return newList;
        }

        private void UpdateMarketSellItemPanelUI(List<Product> products, Action<Product> onProductAction)
        {
            _marketUI.UpdateMarketSellItemPanelUI(products, onProductAction);
        }

        private void UpdateMarketBuyItemPanelUI(List<Product> products, Action<Product> onProductAction)
        {
            foreach (var product in products)
            {
                product.Prize = product.itemFinanceData.buyValue;
                product.canEnoughMoney = _finance.GetCurrentMoney() >= product.itemFinanceData.buyValue;
            }
            _marketUI.UpdateMarketBuyItemPanelUI(products, onProductAction);
        }

        private void UpdateMarketBuyBuildingPanelUI(List<Product> products, Action<Product> onProductAction)
        {
            foreach (var product in products)
            {
                product.Prize = product.itemFinanceData.buyValue;
                product.canEnoughMoney = _finance.GetCurrentMoney() >= product.itemFinanceData.buyValue;
            }
            _marketUI.UpdateMarketBuyBuildingPanelUI(products, onProductAction);
        }
        
        private void OnBuyItem(Product product)
        {
            if (_finance.GetCurrentMoney() >= product.itemFinanceData.buyValue)
            {
                _finance.SubtractMoney(product.itemFinanceData.buyValue);
                _onItemBought.Invoke(product.id, marketSpawnPoint.position);
            }
        }

        private void OnBuyBuilding(Product product)
        {
            if (_finance.GetCurrentMoney() >= product.itemFinanceData.buyValue)
            {
                _finance.SubtractMoney(product.itemFinanceData.buyValue);
                _onBuildingBought.Invoke(product.id);
            }
        }

        private void OnSellProduct(Product product)
        {
            if (_itemsForSell[product].FarmingItem is HarvestProductFarmingItem harvestProductFarmingItem)
            {
                var harvestTaskData = new SellTaskData()
                {
                    plantType =harvestProductFarmingItem.PlantType,
                    sellCount = harvestProductFarmingItem.CurrentStackCount,
                };
                _onUpdateTaskProgress?.Invoke(harvestTaskData);
            }
           
            
            _onItemSold.Invoke(_itemsForSell[product]);
            _itemsForSell.Remove(product);
            _finance.AddMoney(product.itemFinanceData.sellValue);
            
            
        }

        public void TriggerTaskListener(Action<TaskData> updateTaskProgress)
        {
            _onUpdateTaskProgress += updateTaskProgress;
        }

        public void ClearTaskListeners()
        {
            _onUpdateTaskProgress = null;
        }
    }
}
