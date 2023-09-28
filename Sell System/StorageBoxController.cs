using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.Input;
using FarmGame.TimeSystem;
using FarmGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.SellSystem
{
    public class StorageBoxController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputFarm _input;

        [SerializeField]
        private GameObject _storageBoxCanvas;

        [SerializeField]
        private PauseTimeControllerSO _pauseTimeControllerSO;

        [SerializeField]
        private InventoryRendererUI _playerInventoryRenderer, _storageBoxInventoryRenderer;

        [SerializeField]
        private Inventory _storageBoxInventory;
        private Inventory _playerInventory;

        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        [SerializeField]
        private ItemSelectionUI _playerItemSelection, _storageBoxItemSelection;

        [SerializeField]
        private ItemDescriptionUI _itemDescription;

        private Inventory _currentlySelectedInventory;

        [SerializeField]
        private ItemInteractUI _playerItemInteractor, _storageBoxItemInteractor;

        internal void PrepareStorageBox(Inventory inventory)
        {
            _input.EnableUIActionMap();
            _input.OnUIExit += ExitUI;
            _storageBoxCanvas.SetActive(true);

            ConnectToPlayersInventory(inventory);
            PrepareStorageBoxInventory();

            _currentlySelectedInventory = _playerInventory;
            _playerItemSelection.EnableController(_input);

            _playerItemInteractor.EnableController(_input);

            _pauseTimeControllerSO.SetTimePause(true);
        }

        public void TransferItem(ItemSelectionUI selectedWindow)
        {
            Inventory receivingInventory = selectedWindow == _playerItemSelection
                ? _storageBoxInventory : _playerInventory;
            if(_currentlySelectedInventory.GetItemDataAt(selectedWindow.SelectedItem) 
                == null)
            {
                return;
            }

            InventoryItemData item 
                = _currentlySelectedInventory.GetItemDataAt(selectedWindow.SelectedItem);
            ItemDescription itemDescription = _itemDatabase.GetItemData(item.id);

            if (itemDescription != null 
                && receivingInventory.IsThereSpace(item,itemDescription.StackQuantity))
            {
                receivingInventory.AddItem(item, itemDescription.StackQuantity);
                _currentlySelectedInventory.RemoveAllItemAt(selectedWindow.SelectedItem);
            }

            UpdateDescription(selectedWindow.SelectedItem);
        }

        private void PrepareStorageBoxInventory()
        {
            _storageBoxInventory.OnUpdateInventory += UpdateStorageBoxInventoryUI;
            _storageBoxInventoryRenderer.PrepareItemsToShow(_storageBoxInventory.Capacity);
            UpdateStorageBoxInventoryUI(_storageBoxInventory.InventoryContent);
        }

        public void UpdateDescription(int selectedItemIndex)
        {
            InventoryItemData item 
                = _currentlySelectedInventory.GetItemDataAt(selectedItemIndex);
            ItemDescription descriptionData = item == null ? null
                : _itemDatabase.GetItemData(item.id);
            if(descriptionData == null)
            {
                _itemDescription.ResetDescription();
            }
            else
            {
                _itemDescription.UpdateDescription(descriptionData.Image
                    , descriptionData.Name, _itemDatabase.GetItemDescription(item.id));
            }
        }

        public void SwapWindow(ItemSelectionUI selectedWindow)
        {
            _playerInventoryRenderer.ResetAllSelection(true);
            _storageBoxInventoryRenderer.ResetAllSelection(true);

            _playerItemSelection.DisableController(_input);
            _storageBoxItemSelection.DisableController(_input);

            _playerItemInteractor.DisableController(_input);
            _storageBoxItemInteractor.DisableController(_input);

            selectedWindow.EnableController(_input);

            if(selectedWindow == _storageBoxItemSelection)
            {
                _storageBoxItemInteractor.EnableController(_input);
                _currentlySelectedInventory = _storageBoxInventory;
                int itemIndexToSelect
                    = _playerItemSelection.SelectedItem
                    / _playerInventoryRenderer.RowSize
                    * _playerInventoryRenderer.RowSize;
                itemIndexToSelect 
                    = Mathf.Clamp(itemIndexToSelect, 0, _storageBoxInventory.Capacity -1);
                _storageBoxItemSelection.SelectItemAt(itemIndexToSelect);
            }
            else
            {
                _playerItemInteractor.EnableController(_input);
                _currentlySelectedInventory = _playerInventory;
                int itemIndexToSelect = _storageBoxItemSelection.SelectedItem
                    + (_storageBoxInventoryRenderer.RowSize - 1);
                itemIndexToSelect 
                    = Mathf.Clamp(itemIndexToSelect, 0, _playerInventory.Capacity -1);
                _playerItemSelection.SelectItemAt(itemIndexToSelect);
            }
        }

        private void UpdateStorageBoxInventoryUI(IEnumerable<InventoryItemData> inventoryContent)
        {
            UpdateUI(inventoryContent, _storageBoxInventoryRenderer);
        }

        private void UpdateUI(IEnumerable<InventoryItemData> inventoryContent
            , InventoryRendererUI inventoryRenderer)
        {
            inventoryRenderer.ResetItems();
            int index = 0;
            foreach (var item in inventoryContent)
            {
                if(index >= inventoryRenderer.InventoryItemCount)
                {
                    break;
                }
                if(item != null)
                {
                    inventoryRenderer.UpdateItem(index,
                        _itemDatabase.GetItemData(item.id).Image, item.count);
                }
                index++;
            }
        }

        private void ConnectToPlayersInventory(Inventory inventory)
        {
            _playerInventory = inventory;
            _playerInventory.OnUpdateInventory += UpdatePlayersInventoryUI;
            _playerInventoryRenderer
                .PrepareItemsToShow(_playerInventory.Capacity);
            UpdatePlayersInventoryUI(_playerInventory.InventoryContent);
        }

        private void UpdatePlayersInventoryUI(IEnumerable<InventoryItemData> inventoryContent)
        {
            UpdateUI(inventoryContent, _playerInventoryRenderer);
        }

        private void ExitUI()
        {
            _playerItemSelection.DisableController(_input);
            _storageBoxItemSelection.DisableController(_input);
            _playerItemInteractor.DisableController(_input);
            _storageBoxItemInteractor.DisableController(_input);

            _storageBoxCanvas.SetActive(false);
            _pauseTimeControllerSO.SetTimePause(false);
            _input.EnableDefaultActionMap();
            _input.OnUIExit -= ExitUI;
        }
    }
}
