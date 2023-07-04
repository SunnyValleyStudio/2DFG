using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FarmGame.UI
{
    public class ItemSelectionOutlineUI : MonoBehaviour
    {
        [SerializeField]
        private Outline _outline;
        [SerializeField]
        private Color _markColor, _selectColor;

        public bool IsMarked { get; private set; }
        public bool IsSelected { get; private set; }

        public void SetOutline(bool val, Mode mode = Mode.None)
        {
            _outline.enabled = val;
            if(val == false || mode == Mode.None)
            {
                IsMarked = false;
                IsSelected = false;
                _outline.enabled = false;
                return;
            }
            if(mode == Mode.Select)
            {
                _outline.effectColor = _selectColor;
                IsSelected = true;
            }
            if(mode == Mode.Mark)
            {
                _outline.effectColor = _markColor;
                IsMarked = true;
            }
        }
    }

    public enum Mode
    {
        None,
        Select,
        Mark
    }
}
