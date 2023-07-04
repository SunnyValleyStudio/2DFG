using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.UI
{
    public class InventoryItemUpdaterUI : MonoBehaviour
    {
        [SerializeField]
        private InventoryRendererUI _inventoryRenderer;

        public void UpdateElement(int index, ItemDescription itemDescription, 
            InventoryItemData inventoryItem)
        {
            _inventoryRenderer.UpdateItem(index, itemDescription.Image, inventoryItem.count);
        }

        public void ClearElements() => _inventoryRenderer.ResetItems();
    }
}
