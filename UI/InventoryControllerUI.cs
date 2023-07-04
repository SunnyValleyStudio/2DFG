using FarmGame.DataStorage.Inventory;
using FarmGame.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.UI
{
    public class InventoryControllerUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputFarm _input;
        [SerializeField]
        private GameObject _inventoryCanvas;

        public void ShowInventory(Inventory inventory)
        {
            _inventoryCanvas.SetActive(true);
            _input.EnableUIActionMap();
            _input.OnUIExit += ExitInventory;
            _input.OnUIToggleInventory += ExitInventory;
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
