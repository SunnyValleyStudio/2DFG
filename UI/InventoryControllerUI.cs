using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.UI
{
    [RequireComponent(typeof(InventoryRendererUI))]
    public class InventoryControllerUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputFarm _input;
        [SerializeField]
        private GameObject _inventoryCanvas;

        [SerializeField]
        private InventoryItemUpdaterUI _inventoryItemUpdater;
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        private InventoryRendererUI _inventoryRenderer;

        private void Awake()
        {
            _inventoryRenderer = GetComponent<InventoryRendererUI>();
        }

        public void ShowInventory(Inventory inventory)
        {
            _inventoryCanvas.SetActive(true);
            _input.EnableUIActionMap();
            _input.OnUIExit += ExitInventory;
            _input.OnUIToggleInventory += ExitInventory;

            _inventoryRenderer.PrepareItemsToShow(inventory.Capacity);
            _inventoryRenderer.ResetItems();

            UpdateInventoryItems(inventory.InventoryContent);
        }

        private void UpdateInventoryItems(IEnumerable<InventoryItemData> inventoryContent)
        {
            _inventoryRenderer.ResetItems();
            int index = 0;
            foreach (InventoryItemData item in inventoryContent)
            {
                if(item != null)
                {
                    _inventoryItemUpdater.UpdateElement(index, _itemDatabase.GetItemData(item.id),
                        item);
                }
                index++;
            }
        }

        private void ExitInventory()
        {
            _input.EnableDefaultActionMap();
            _input.OnUIExit -= ExitInventory;
            _input.OnUIToggleInventory -= ExitInventory;
            _inventoryCanvas.SetActive(false);
        }
    }
}
