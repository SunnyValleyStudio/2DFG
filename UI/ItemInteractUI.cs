using FarmGame.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.UI
{
    public class ItemInteractUI : MonoBehaviour
    {
        [SerializeField]
        private ItemSelectionUI _itemSelector;

        public UnityEvent<ItemSelectionUI> OnItemSelected;

        public void InteractWithItem()
        {
            OnItemSelected?.Invoke(_itemSelector);
        }

        public void EnableController(PlayerInputFarm input)
        {
            input.OnUIInteract += InteractWithItem;
        }

        public void DisableController(PlayerInputFarm input)
        {
            input.OnUIInteract -= InteractWithItem;
        }
    }
}
