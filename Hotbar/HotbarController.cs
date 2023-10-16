using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Hotbar
{
    public class HotbarController : MonoBehaviour
    {
        private ItemControllerUI[] _hotbarItems = null;
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        private void Awake()
        {
            _hotbarItems = new ItemControllerUI[transform.childCount];
            int index = 0;
            foreach (Transform item in transform)
            {
                ItemControllerUI itemController = item.GetComponent<ItemControllerUI>();
                if (itemController != null)
                {
                    _hotbarItems[index] = itemController;
                    index++;
                }
            }
        }

        public void UpdateHotBar(IEnumerable<InventoryItemData> inventoryContent)
        {
            foreach (var item in _hotbarItems)
            {
                item.ResetData();
            }

            int index = 0;
            foreach (var item in inventoryContent)
            {
                if (index >= _hotbarItems.Length)
                {
                    break;
                }

                if (item != null)
                {
                    _hotbarItems[index]
                        .UpdateData(_itemDatabase.GetItemData(item.id).Image,
                        item.count);
                }
                index++;
            }
        }

        public void SelectItem(int index)
        {
            ResetSelection();
            if (_hotbarItems.Length < index || index < 0)
            {
                return;
            }
            ItemSelectionOutlineUI outline
                = _hotbarItems[index].GetComponent<ItemSelectionOutlineUI>();
            if (outline != null)
            {
                outline.SetOutline(true, Mode.Select);
            }
        }

        private void ResetSelection()
        {
            for (int i = 0; i < _hotbarItems.Length; i++)
            {
                ItemSelectionOutlineUI outline
                    = _hotbarItems[i].GetComponent<ItemSelectionOutlineUI>();
                if (outline != null)
                {
                    outline.SetOutline(false);
                }
            }
        }
    }
}
