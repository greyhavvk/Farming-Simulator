using System;
using System.Collections.Generic;
using Systems.MarketSystem;
using UnityEngine;

namespace Core.UIManager.Market
{
    public class MarketUIPanelController:MonoBehaviour,IMarketUI
    {
        [SerializeField] private GameObject marketUIPanelParent;
        [SerializeField] private MarketUIPanel itemPurchasePanel;
        [SerializeField] private MarketUIPanel buildingPurchasePanel;
        [SerializeField] private MarketUIPanel itemSellPanel;

        public void Initialize()
        {
            itemPurchasePanel.Initialize();
            buildingPurchasePanel.Initialize();
            itemSellPanel.Initialize();
        }
        
        public void ShowItemPurchasePanel()
        {
            CloseAllPanels();
            itemPurchasePanel.gameObject.SetActive(true);
        }

        public void ShowBuildingPurchasePanel()
        {
            CloseAllPanels();
            buildingPurchasePanel.gameObject.SetActive(true);
        }

        public void ShowItemSellPanel()
        {
            CloseAllPanels();
            itemSellPanel.gameObject.SetActive(true);
        }

        private void CloseAllPanels()
        {
            itemPurchasePanel.gameObject.SetActive(false);
            buildingPurchasePanel.gameObject.SetActive(false);
            itemSellPanel.gameObject.SetActive(false);
        }

        public void UpdateMarketSellItemPanelUI(List<Product> products, Action<Product> onProductAction)
        {
            itemSellPanel.UpdateMarketPanel(products, onProductAction);
        }

        public void UpdateMarketBuyItemPanelUI(List<Product> products, Action<Product> onProductAction)
        {
            itemPurchasePanel .UpdateMarketPanel(products, onProductAction);
        }

        public void UpdateMarketBuyBuildingPanelUI(List<Product> products, Action<Product> onProductAction)
        {
            buildingPurchasePanel.UpdateMarketPanel(products, onProductAction);
        }

        public void OpenMarket()
        {
            marketUIPanelParent.SetActive(true);
            ShowItemPurchasePanel();
        }

        public void CloseMarket()
        {
            marketUIPanelParent.SetActive(false);
        }
    }
}