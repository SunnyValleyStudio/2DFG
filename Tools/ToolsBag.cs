using FarmGame.Agent;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class ToolsBag : MonoBehaviour
    {
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        private int _selectedIndex = 0;

        [SerializeField]
        private List<int> _initialTools;
        [SerializeField]
        private Inventory _toolsBagInventory;

        private List<Tool> _newBag;

        [SerializeField]
        private int _handToolID = 4;

        public Tool CurrentTool => _newBag[_selectedIndex];
        public event Action<int, List<Sprite>, int?> OnToolsBagUpdated;

        private void Start()
        {
            for (int i = 0; i < _initialTools.Count; i++)
            {
                ItemDescription description = _itemDatabase.GetItemData(_initialTools[i]);
                _toolsBagInventory.AddItem(new InventoryItemData(description.ID, 1, -1, null),
                    description.StackQuantity);
            }
            UpdateToolsBag(_toolsBagInventory.InventoryContent);
        }

        private void UpdateToolsBag(IEnumerable<InventoryItemData> inventoryContent)
        {
            _newBag = new();
            AddDefaultHandTool();
            foreach (InventoryItemData tool in inventoryContent) 
            { 
                if(tool != null)
                {
                    ItemDescription toolDescription = _itemDatabase.GetItemData(tool.id);
                    if(toolDescription == null || toolDescription.ToolType == ToolType.None)
                    {
                        Debug.LogError($"LOaded tool with index {tool.id} is not present in database or None");
                    }
                    Tool newTool = ToolFactory.CreateTool(toolDescription, tool.data);
                    _newBag.Add(newTool);
                }
            }
            if(_selectedIndex >= _newBag.Count)
                _selectedIndex = 0;
        }

        private void AddDefaultHandTool()
        {
            ItemDescription handToolDescription = _itemDatabase.GetItemData(_handToolID);
            Tool handTool = ToolFactory.CreateTool(handToolDescription, null);
            _newBag.Add(handTool); // handTOol -> 0
        }

        public void Initialize(IAgent agent)
        {
            SwapTool(_selectedIndex, agent);
        }

        public void SelectNextTool(IAgent agent)
        {
            SwapTool(_selectedIndex + 1, agent);
        }

        private void SwapTool(int newIndex, IAgent agent)
        {
            if(_newBag[_selectedIndex] != null)
            {
                PutAway(agent);
            }
            _selectedIndex = newIndex;
            if(_selectedIndex >= _newBag.Count)
            {
                _selectedIndex = 0;
            }
            ItemDescription description 
                = _itemDatabase.GetItemData(_newBag[_selectedIndex].ItemIndex);
            Debug.Log($" Equipped tool: {description.Name}");
            //_newBag[_selectedIndex] = ToolFactory.CreateTool(description);
            EquipTool(agent);
            SendUpdateMessage();
        }

        private void SendUpdateMessage()
        {
            List<Sprite> sprites = new List<Sprite>();
            foreach (Tool tool in _newBag)
            {
                ItemDescription toolDescription = _itemDatabase.GetItemData(tool.ItemIndex);
                if(toolDescription != null)
                {
                    sprites.Add(toolDescription.Image);
                }
            }
            OnToolsBagUpdated?.Invoke(_selectedIndex, sprites, null);
        }

        private void EquipTool(IAgent agent)
        {
            _newBag[_selectedIndex].Equip(agent);
        }

        private void PutAway(IAgent agent)
        {
            _newBag[_selectedIndex].PutAway(agent);
        }
    }
}
