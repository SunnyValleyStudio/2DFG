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
