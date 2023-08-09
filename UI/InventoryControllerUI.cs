using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.Input;
using FarmGame.TimeSystem;
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

        private Inventory _inventoryTempReference;
        [SerializeField]
        private ItemSelectionUI _itemSelection;
        [SerializeField]
        private ItemDescriptionUI _itemDescription;
        [SerializeField]
        private PauseTimeControllerSO _pauseTimeController;

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

            _inventoryTempReference = inventory;
            _inventoryTempReference.OnUpdateInventory += UpdateInventoryItems;
            _itemSelection.EnableController(_input);

            UpdateInventoryItems(inventory.InventoryContent);
            _pauseTimeController.SetTimePause(true);
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

        public void UpdateDescription(int selectedItemIndex)
        {
            InventoryItemData item = _inventoryTempReference.GetItemDataAt(selectedItemIndex);
            ItemDescription descriptionData = item == null ? null :
                _itemDatabase.GetItemData(item.id);
            if (descriptionData == null)
                _itemDescription.ResetDescription();
            else
                _itemDescription.UpdateDescription(descriptionData.Image, descriptionData.Name,
                    _itemDatabase.GetItemDescription(item.id));
        }

        private void ExitInventory()
        {
            _pauseTimeController.SetTimePause(false);
            _input.EnableDefaultActionMap();
            _input.OnUIExit -= ExitInventory;
            _input.OnUIToggleInventory -= ExitInventory;
            _inventoryCanvas.SetActive(false);
            _inventoryTempReference.OnUpdateInventory -= UpdateInventoryItems;
            _itemSelection.DisableController(_input);
        }
    }
}
