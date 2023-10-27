using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FarmGame.Store
{
    public class ItemPriceUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _priceText;

        public void SetPrice(int priece)
        {
            _priceText.text = $"{priece} $";
        }
    }
}
