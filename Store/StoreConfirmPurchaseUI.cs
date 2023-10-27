using FarmGame.DataStorage;
using FarmGame.Input;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarmGame.Store
{
    public class StoreConfirmPurchaseUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _quantityText, _endPriceText, _unitPriceText, _playerMoneyText;
        [SerializeField]
        private Button _confirmButton;
        private int _unitPrice;
        private int _playerMoney;
        public int Quantity { get; private set; }

        internal void HideConfirmWindow(PlayerInputFarm input)
        {
            input.OnUIMoveInput -= ChangeQuantity;
            gameObject.SetActive(false);
        }

        private void ChangeQuantity(Vector2 arrowInput)
        {
            if(arrowInput.y > 0)
            {
                Quantity++;
                UpdateQuantityAndPrice();
            }
            if(arrowInput.y < 0)
            {
                if(Quantity > 1)
                    Quantity--;
                UpdateQuantityAndPrice();
            }

        }

        private void UpdateQuantityAndPrice()
        {
            int endPrice = Quantity * _unitPrice;
            _endPriceText.text = $"{endPrice}";
            _quantityText.text = Quantity.ToString();
            if(endPrice > _playerMoney)
            {
                _confirmButton.image.color = Color.red;
                _endPriceText.color = Color.red;
            }
            else
            {
                _confirmButton.image.color = Color.green;
                _endPriceText.color = Color.green;
            }
        }

        internal void ShowConfirmUI(PlayerInputFarm input, ItemDescription description,
            int money)
        {
            Quantity = 1;
            gameObject.SetActive(true);
            _playerMoney = money;
            _unitPrice = description.Price;
            _unitPriceText.text = _unitPrice.ToString();
            _playerMoneyText.text = _playerMoney.ToString();
            input.OnUIMoveInput += ChangeQuantity;

            UpdateQuantityAndPrice();
        }
    }
}