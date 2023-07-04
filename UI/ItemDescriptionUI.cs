using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarmGame.UI
{
    public class ItemDescriptionUI : MonoBehaviour
    {
        [SerializeField]
        private Image _itemImage;
        [SerializeField]
        private TextMeshProUGUI _itemNameTxt, _itemDescriptionTxt;

        public void UpdateDescription(Sprite sprite, string itemName, string description)
        {
            Color c = Color.white;
            if (sprite == null)
                c.a = 0;
            _itemImage.color = c;
            _itemImage.sprite = sprite;
            _itemNameTxt.text = itemName;
            _itemDescriptionTxt.text = description;
        }

        public void ResetDescription()
        {
            UpdateDescription(null,string.Empty,string.Empty);
        }
    }
}
