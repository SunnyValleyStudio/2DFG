using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.Hotbar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.CarryItemManager
{
    public class CarryItemSystem : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private int _currentSelectedItemIndex = -1;

        [SerializeField]
        private ItemDatabaseSO _itemDatabaseSO;

        public bool AmICarryingItem => _currentSelectedItemIndex > -1;
        public int CarriedItemIndex => _currentSelectedItemIndex;

        [SerializeField]
        private HotbarController _hotbar;

        public event Action OnStartCarrying, OnCancelCarrying;

        public bool StartCarrying(int index, Inventory inventory)
        {
            InventoryItemData carriedItem = inventory.GetItemDataAt(index);
            if (carriedItem == null)
            {
                ResetSelection();
                return false;
            }

            if(_currentSelectedItemIndex != index)
            {
                ItemDescription itemDescription 
                    = _itemDatabaseSO.GetItemData(carriedItem.id);
                _currentSelectedItemIndex = index;
                _spriteRenderer.gameObject.SetActive(true);
                _hotbar.SelectItem(index);
                _spriteRenderer.sprite = itemDescription.Image;
            }
            OnStartCarrying?.Invoke();
            return false;
        }

        public void ResetSelection()
        {
            OnCancelCarrying?.Invoke();
            _spriteRenderer.gameObject.SetActive(false);
            _currentSelectedItemIndex = -1;
            _hotbar.ResetSelection();
        }
    }
}
