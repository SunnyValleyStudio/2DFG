using FarmGame.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.UI
{
    public class ItemSwapperUI : MonoBehaviour
    {
        [SerializeField]
        private InventoryRendererUI _inventoryRendererUI;
        [SerializeField]
        private ItemSelectionUI _itemSelectionUI;

        public UnityEvent<int> OnItemSelected;
        public UnityEvent<int> OnItemSwapped;
        public UnityEvent<int, int> OnItemSwapBetween;

        int? _itemIndexSelectedForInteraction = null;

        public int? ItemIndexSelectedForInteraction
        {
            get => _itemIndexSelectedForInteraction;
            set
            {
                _itemIndexSelectedForInteraction = value;
                if(_itemIndexSelectedForInteraction.HasValue) 
                {
                    _inventoryRendererUI.MarkItem(_itemIndexSelectedForInteraction.Value);
                }
            }
        }

        public void ConfirmSelection(int selectedIndex)
        {
            ItemIndexSelectedForInteraction = _itemSelectionUI.SelectedItem;
            Debug.Log($"Confirmed selection of item {_itemSelectionUI.SelectedItem}. " +
                $"Item is ready for swapping");
        }

        public void InteractWithItem()
        {
            if(ItemIndexSelectedForInteraction == null)
            {
                OnItemSelected?.Invoke(_itemSelectionUI.SelectedItem);
                Debug.Log($"Checking selected item {_itemSelectionUI.SelectedItem} if can be swapped");
                return;
            }
            else
            {
                OnItemSwapBetween?.Invoke(ItemIndexSelectedForInteraction.Value,
                    _itemSelectionUI.SelectedItem);
                _inventoryRendererUI.ResetMarkedSelection(ItemIndexSelectedForInteraction.Value);

                OnItemSwapped?.Invoke(_itemSelectionUI.SelectedItem);
                ItemIndexSelectedForInteraction = null;
            }
        }

        public void EnableController(PlayerInputFarm input)
        {
            ItemIndexSelectedForInteraction = null;
            input.OnUIInteract += InteractWithItem;
        }

        public void DisableController(PlayerInputFarm input)
        {
            if(ItemIndexSelectedForInteraction != null)
            {
                _inventoryRendererUI.ResetMarkedSelection(_itemIndexSelectedForInteraction.Value);
            }
            input.OnUIInteract -= InteractWithItem;
        }
    }
}
