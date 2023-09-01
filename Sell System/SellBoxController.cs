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
    public class SellBoxController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputFarm _input;

        [SerializeField]
        private GameObject _sellBoxCanvas;

        [SerializeField]
        private PauseTimeControllerSO _pauseTimeControllerSO;

        [SerializeField]
        private InventoryRendererUI _playerInventoryRenderer, _sellBoxInventoryRenderer;

        [SerializeField]
        private Inventory _sellBoxInventory;
        private Inventory _playerInventory;

        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        [SerializeField]
        private ItemSelectionUI _playerItemSelection, _sellBoxItemSelection;

        [SerializeField]
        private ItemDescriptionUI _itemDescription;

        private Inventory _currentlySelectedInventory;

        internal void PrepareSellBox(Inventory inventory)
        {
            _input.EnableUIActionMap();
            _input.OnUIExit += ExitUI;
            _sellBoxCanvas.SetActive(true);

            ConnectToPlayersInventory(inventory);
            PrepareSellBoxInventory();

            _currentlySelectedInventory = _playerInventory;
            _playerItemSelection.EnableController(_input);

            _pauseTimeControllerSO.SetTimePause(true);
        }

        private void PrepareSellBoxInventory()
        {
            _sellBoxInventory.OnUpdateInventory += UpdateSellBoxInventoryUI;
            _sellBoxInventoryRenderer.PrepareItemsToShow(_sellBoxInventory.Capacity);
            UpdateSellBoxInventoryUI(_sellBoxInventory.InventoryContent);
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
            _sellBoxInventoryRenderer.ResetAllSelection(true);

            _playerItemSelection.DisableController(_input);
            _sellBoxItemSelection.DisableController(_input);

            selectedWindow.EnableController(_input);

            if(selectedWindow == _sellBoxItemSelection)
            {
                _currentlySelectedInventory = _sellBoxInventory;
                int itemIndexToSelect
                    = _playerItemSelection.SelectedItem
                    / _playerInventoryRenderer.RowSize
                    * _playerInventoryRenderer.RowSize;
                itemIndexToSelect 
                    = Mathf.Clamp(itemIndexToSelect, 0, _sellBoxInventory.Capacity -1);
                _sellBoxItemSelection.SelectItemAt(itemIndexToSelect);
            }
            else
            {
                _currentlySelectedInventory = _playerInventory;
                int itemIndexToSelect = _sellBoxItemSelection.SelectedItem
                    + (_sellBoxInventoryRenderer.RowSize - 1);
                itemIndexToSelect 
                    = Mathf.Clamp(itemIndexToSelect, 0, _playerInventory.Capacity -1);
                _playerItemSelection.SelectItemAt(itemIndexToSelect);
            }
        }

        private void UpdateSellBoxInventoryUI(IEnumerable<InventoryItemData> inventoryContent)
        {
            UpdateUI(inventoryContent, _sellBoxInventoryRenderer);
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
            _sellBoxItemSelection.DisableController(_input);

            _sellBoxCanvas.SetActive(false);
            _pauseTimeControllerSO.SetTimePause(false);
            _input.EnableDefaultActionMap();
            _input.OnUIExit -= ExitUI;
        }
    }
}
