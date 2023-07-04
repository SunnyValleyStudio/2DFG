using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarmGame.UI
{
    [RequireComponent(typeof(ItemSelectionOutlineUI))]
    public class ItemControllerUI : MonoBehaviour
    {
        [SerializeField]
        private Image _itemImage;
        [SerializeField]
        private TextMeshProUGUI _quantityTxt;
        public ItemSelectionOutlineUI Outline { get; private set; }

        private void Awake()
        {
            Outline = GetComponent<ItemSelectionOutlineUI>();
        }
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
