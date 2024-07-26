using System;
using System.Collections.Generic;
using Systems.MarketSystem;
using UnityEngine;

namespace Core.UIManager.Market
{
    public class MarketUIPanel:MonoBehaviour
    {
        [SerializeField] private ProductUIButton refButton;
        [SerializeField] private Transform buttonParent;
        private List<ProductUIButton> _existingButtons;

        public void Initialize()
        {
            _existingButtons = new List<ProductUIButton>();
        }
        
        public void UpdateMarketPanel(List<Product> products, Action<Product> onProductAction)
        {
            var neededNewButtonCount = products.Count - _existingButtons.Count;
            if (neededNewButtonCount>0)
            {
                CreateNewButtons(neededNewButtonCount);
            }

            for (int i = 0; i < _existingButtons.Count; i++)
            {
                _existingButtons[i].gameObject.SetActive(i<products.Count);
                if (i<products.Count)
                {
                    _existingButtons[i].UpdateButton(products[i], onProductAction);
                }
            }
        }

        private void CreateNewButtons(int neededNewButtonCount)
        {
            for (int i = 0; i < neededNewButtonCount; i++)
            {
                var newButton = Instantiate(refButton, buttonParent);
                _existingButtons.Add(newButton);
            }
        }
    }
}