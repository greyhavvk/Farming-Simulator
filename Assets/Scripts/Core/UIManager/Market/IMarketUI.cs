using System;
using System.Collections.Generic;
using Systems.MarketSystem;

namespace Core.UIManager.Market
{
    public interface IMarketUI
    {
        void UpdateMarketSellItemPanelUI(List<Product> products, Action<Product> onProductAction);
        void UpdateMarketBuyItemPanelUI(List<Product> products, Action<Product> onProductAction);
        void UpdateMarketBuyBuildingPanelUI(List<Product> products, Action<Product> onProductAction);
        void OpenMarket();
        void CloseMarket();
    }
}