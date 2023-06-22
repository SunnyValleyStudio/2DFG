using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarmGame.UI
{
    public class ToolSelectionUI : MonoBehaviour
    {
        [SerializeField]
        private Image _toolImage;
        [SerializeField]
        private Image _toolTipImage;
        [SerializeField]
        private List<Image> _toolImages;
        [SerializeField]
        private TextMeshProUGUI _countTxt;
        [SerializeField]
        private float _alphaOfEmptyImage = 0.04f, _alphaOfFilledImage = 0.5f;

        public void UpdateUI(int selectedImageIndex, List<Sprite> images, int? count)
        {
            ClearToolsList();
            Color filledImageColor = Color.white;
            filledImageColor.a = _alphaOfFilledImage;
            ToggleToolSwapTip(images.Count > 1);

            for (int i = 0; i < images.Count; i++)
            {
                if (i >= _toolImages.Count)
                    break;
                if(i == selectedImageIndex)
                {
                    _toolImage.sprite = images[i];
                    if (count.HasValue)
                    {
                        _countTxt.gameObject.SetActive(true);
                        _countTxt.text = count.Value.ToString();
                    }
                    else
                    {
                        _countTxt.gameObject.SetActive(false);
                    }
                }
                if(_toolImages.Count > i && images[i] != null)
                {
                    _toolImages[i].sprite = images[i];
                    _toolImages[i].color = filledImageColor;
                }
            }
        }

        private void ToggleToolSwapTip(bool val)
        {
            _toolTipImage.gameObject.SetActive(val);
        }

        private void ClearToolsList()
        {
            Color c = Color.white;
            c.a = _alphaOfEmptyImage;
            foreach (var image in _toolImages)
            {
                image.sprite = null;
                image.color = c;
            }
        }
    }
}
