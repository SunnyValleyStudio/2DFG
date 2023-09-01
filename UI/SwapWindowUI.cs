using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.UI
{
    public class SwapWindowUI : MonoBehaviour
    {
        [SerializeField]
        private Direction _swapDirection;

        [SerializeField]
        private ItemSelectionUI _selectionWindow;

        public UnityEvent<ItemSelectionUI> OnSwapWindow;

        public void TrySwappingWindow(Direction direction, int index)
        {
            if (direction == _swapDirection)
            {
                OnSwapWindow?.Invoke(_selectionWindow);
            }
        }
    }
}
