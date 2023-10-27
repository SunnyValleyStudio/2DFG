using FarmGame.Agent;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.Input;
using FarmGame.TimeSystem;
using FarmGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Store
{
    public class StoreUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputFarm _input;
        [SerializeField]
        private PauseTimeControllerSO _pauseTimeController;
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        [SerializeField]
        private InventoryRendererUI _storeRenderer;
        [SerializeField]
        private ItemSelectionUI _storeItemSelection;
        [SerializeField]
        private ItemDescriptionUI _storeItemDescription;
        [SerializeField]
        private ItemInteractUI _storeItemInteraction;

        [SerializeField]
        private StoreConfirmPurchaseUI _storeConfirmUI;

        private Inventory _storeInventyory;
        private IAgent _agent;

        private int _lastSelection = 0;

        public UnityEvent<PurchasedItemData, IAgent> OnItemPurchased;
        public event Action OnCloseStoreUI;

        public void PrepareStoreUI(IAgent agent, Inventory storeInventory)
        {
            _storeInventyory = storeInventory;
            _agent = agent;

            _input.EnableUIActionMap();

            _pauseTimeController.SetTimePause(true);

            ShowStoreUI(storeInventory);
        }

        private void ShowStoreUI(Inventory storeInventory)
        {
            gameObject.SetActive(true);
            _storeInventyory.OnUpdateInventory += UpdateStoreInventoryUI;
            _storeRenderer.PrepareItemsToShow(storeInventory.Capacity);

            ConnectToInput();

            UpdateStoreInventoryUI(storeInventory.InventoryContent);
        }

        private void ConnectToInput()
        {
            _input.OnUIExit += ExitStoreUI;
            _storeItemInteraction.EnableController(_input);
            _storeItemSelection.EnableController(_input);
        }

        private void DisconnectFromInput()
        {
            _input.OnUIExit -= ExitStoreUI;
            _storeItemInteraction.DisableController(_input);
            _storeItemSelection.DisableController(_input);
        }

        private void ExitStoreUI()
        {
            DisconnectFromInput();
            gameObject.SetActive(false);
            _pauseTimeController.SetTimePause(false);
            _input.EnableDefaultActionMap();

            OnCloseStoreUI?.Invoke();
        }

        private void UpdateStoreInventoryUI(IEnumerable<InventoryItemData> inventoryContent)
        {
            _storeRenderer.ResetItems();
            int index = 0;
            foreach (InventoryItemData item in inventoryContent)
            {
                if (index >= _storeRenderer.InventoryItemCount)
                    break;
                if (item != null)
                {
                    ItemDescription itemData = _itemDatabase.GetItemData(item.id);
                    _storeRenderer.UpdateItem(index, itemData.Image, item.count);
                    ItemControllerUI itemController = _storeRenderer.GetItemAt(index);
                    if(itemController != null)
                    {
                        itemController.GetComponent<ItemPriceUI>().SetPrice(itemData.Price);
                    }
                }
                index++;    
            }
        }

        public void UpdateDescription(int selectedItemIndex)
        {
            InventoryItemData item = _storeInventyory.GetItemDataAt(selectedItemIndex);
            ItemDescription descriptionData = item == null ? null :
                _itemDatabase.GetItemData(item.id);
            if (descriptionData == null)
                _storeItemDescription.ResetDescription();
            else
                _storeItemDescription.UpdateDescription(descriptionData.Image, descriptionData.Name,
                    _itemDatabase.GetItemDescription(item.id));
        }

        public void ShowSellWindow(ItemSelectionUI selection)
        {
            ItemDescription description 
                = _itemDatabase
                .GetItemData(_storeInventyory.GetItemDataAt(selection.SelectedItem).id);
            _lastSelection = _storeItemSelection.SelectedItem;

            DisconnectFromInput();

            _input.OnUIExit += OnExitSellWindow;
            _input.OnUIInteract += PurchaseSelectedItem;
            _storeConfirmUI.ShowConfirmUI(_input, description, _agent.AgentData.Money);
        }

        private void PurchaseSelectedItem()
        {
            PurchasedItemData data = new()
            {
                ID = _storeInventyory.GetItemDataAt(_storeItemSelection.SelectedItem).id,
                quantity = _storeConfirmUI.Quantity
            };
            OnItemPurchased?.Invoke(data, _agent);
            OnExitSellWindow();
        }

        private void OnExitSellWindow()
        {
            ConnectToInput();

            _input.OnUIExit -= OnExitSellWindow;
            _input.OnUIInteract -= PurchaseSelectedItem;

            _storeConfirmUI.HideConfirmWindow();

            _storeItemSelection.SelectItemAt(_lastSelection);
        }
    }

    public struct PurchasedItemData
    {
        public int ID;
        public int quantity;
    }
}
