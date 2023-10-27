using FarmGame.Agent;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Store
{
    public class StoreController : MonoBehaviour
    {
        [SerializeField]
        private Inventory _storeInventory;
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;
        [SerializeField]
        private Inventory _toolBoxInventory, _playerToolsInventory;
        [SerializeField]
        private StoreUI _storeUI;

        private bool _showAdditionalFeedback = false;
        public UnityEvent OnToolTransferedToBox;

        private void Start()
        {
            _storeInventory.ChangeCapacity(3);
            _storeInventory.AddItem(new InventoryItemData(2, 1, -1), 0);
            _storeInventory.AddItem(new InventoryItemData(6, 1, -1), 0);
            _storeInventory.AddItem(new InventoryItemData(7, 1, -1), 0);
            
        }

        private void ShowFeedback()
        {
            _storeUI.OnCloseStoreUI -= ShowFeedback;
            if (_showAdditionalFeedback)
            {
                _showAdditionalFeedback = false;
                OnToolTransferedToBox?.Invoke();
            }
        }

        public void ShowUI(IAgent agent)
        {
            if(_storeInventory.Capacity <= 0)
            {
                Start();
            }
            _storeUI.OnCloseStoreUI += ShowFeedback;

            _storeUI.PrepareStoreUI(agent, _storeInventory);
        }

        public void MakePurchase(PurchasedItemData data, IAgent agent)
        {
            ItemDescription description = _itemDatabase.GetItemData(data.ID);
            int endPrince = data.quantity * description.Price;
            if(endPrince > agent.AgentData.Money)
            {
                return;
            }
            agent.AgentData.Money -= endPrince;

            if(description.ToolType != Tools.ToolType.None)
            {
                Debug.Log($"Purchasing a tool {description.ToolType}");
                AddToToolBag(data.quantity,description, agent);
            }
            else
            {
                Debug.Log($"Purchasing an item {data.ID}");
                AddToInventory(data.quantity, description, agent);
            }
        }

        private void AddToInventory(int quantity, ItemDescription description, IAgent agent)
        {
            InventoryItemData newItem = new InventoryItemData(description.ID, quantity, -1);
            agent.Inventory.AddItem(newItem, description.StackQuantity);
        }

        private void AddToToolBag(int quantity, ItemDescription description, IAgent agent)
        {
            InventoryItemData newItem = new InventoryItemData(description.ID, quantity,
                -1, ToolFactory.GetToolData(description,quantity));
            if (_playerToolsInventory.IsThereSpace(newItem, description.StackQuantity))
            {
                _playerToolsInventory.AddItem(newItem,description.StackQuantity);
            }
            else
            {
                _toolBoxInventory.AddItem(newItem, description.StackQuantity);
                _showAdditionalFeedback = true;
            }
        }
    }
}
