using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarmGame.UI
{
    public class ItemControllerUI : MonoBehaviour
    {
        [SerializeField]
        private Image _itemImage;
        [SerializeField]
        private TextMeshProUGUI _quantityTxt;

        public void ResetData()
        {
            _quantityTxt.enabled = false;
            _itemImage.enabled = false;
        }

        public void UpdateData(Sprite image, int quantity)
        {
            _quantityTxt.enabled=true;
            _quantityTxt.text = quantity.ToString();
            _itemImage.enabled=true;
            _itemImage.sprite=image;
        }
    }
}
