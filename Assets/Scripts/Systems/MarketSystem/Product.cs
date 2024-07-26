using Systems.FinanceSystem;
using UnityEngine;

namespace Systems.MarketSystem
{
    [System.Serializable]
    public class Product
    {
        public ItemFinanceData itemFinanceData;
        public Sprite productSprite;
        public int id;
        public bool canEnoughMoney;
        public int stack;

        public int Prize
        {
            get;
            set;
        }
    }
}