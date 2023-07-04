using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.UI
{
    public class InventoryRendererUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _inventoryUIItemPrefab;
        [SerializeField]
        private Transform _inventoryItemParent;

        List<ItemControllerUI> _inventoryItems = new();

        [SerializeField]
        private int _rowSize = 7;
        public int RowSize { get => _rowSize; }
        public int InventoryItemCount => _inventoryItems.Count;

        public void PrepareItemsToShow(int capacity)
        {
            foreach (Transform item in _inventoryItemParent)
            {
                Destroy(item.gameObject);
            }

            _inventoryItems.Clear();

            for (int i = 0; i < capacity; i++)
            {
                GameObject inventoryItem = Instantiate(_inventoryUIItemPrefab);
                inventoryItem.transform.SetParent(_inventoryItemParent);
                ItemControllerUI itemController = inventoryItem.GetComponent<ItemControllerUI>();
                _inventoryItems.Add(itemController);
            }
        }

        public void UpdateItem(int index, Sprite sprite, int itemCount)
        {
            GetItemAt(index).UpdateData(sprite, itemCount);
        }

        private ItemControllerUI GetItemAt(int index)
        {
            if (index >= _inventoryItems.Count || index < 0)
                throw new IndexOutOfRangeException($"There is no UI item with index {index}");
            return _inventoryItems[index];
        }

        public void ResetItems()
        {
            foreach (var item in _inventoryItems)
            {
                item.ResetData();
            }
        }

        internal void SelectItem(int selctedItemIndex)
        {
            GetItemAt(selctedItemIndex).Outline.SetOutline(true, Mode.Select);
        }

        internal void ResetAllSelection(bool v)
        {
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                ItemControllerUI controller = GetItemAt(i);
                if(v == false && controller.Outline.IsMarked)
                {
                    MarkItem(i, true);
                    continue;
                }
                controller.Outline.SetOutline(false);
            }
        }

        private void MarkItem(int itemIndex, bool resetSelection = true)
        {
            ItemControllerUI controller = GetItemAt(itemIndex);
            if (resetSelection)
                controller.Outline.SetOutline(false);
            controller.Outline.SetOutline(true, Mode.Mark);
        }
    }
}
