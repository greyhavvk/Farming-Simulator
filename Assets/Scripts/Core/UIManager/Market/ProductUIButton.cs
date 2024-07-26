using System;
using Systems.MarketSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UIManager.Market
{
    public class ProductUIButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Image productImage;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text stackText;
        private Product _product;

        public void UpdateButton(Product product, Action<Product> onProductAction)
        {
            _product = product;
            priceText.text = product.Prize.ToString();
            productImage.sprite = product.productSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onProductAction?.Invoke(_product));
            button.interactable = product.canEnoughMoney;
            priceText.color = button.interactable ? Color.black : Color.red;
            if (product.stack>0)
            {
                stackText.gameObject.SetActive(true);
                stackText.text = product.stack.ToString();
            }
            else
            {
                stackText.gameObject.SetActive(false);
            }
        }
    }
}