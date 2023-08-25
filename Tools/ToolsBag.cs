using FarmGame.Agent;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                string data = null;
                int quantity = 1;
                if(description.ToolType == ToolType.SeedPlacer)
                {
                    data = JsonUtility.ToJson(new SeedToolData
                    {
                        cropID = description.CropTypeIndex,
                        quantity = 2
                    });
                    quantity = 2;
                }
                _toolsBagInventory.AddItem(new InventoryItemData(description.ID, quantity, 
                    -1, data), description.StackQuantity);
            }
            UpdateToolsBag(_toolsBagInventory.InventoryContent);
        }

        private void UpdateToolsBag(IEnumerable<InventoryItemData> inventoryContent)
        {
            _newBag = new();
            AddDefaultHandTool();
            int index = 0;
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
                    if(newTool is IQuantity)
                    {
                        ((IQuantity)newTool).Quantity = tool.count;
                        _toolsBagInventory.AddItemAt(index,
                            new InventoryItemData(toolDescription.ID, tool.count, tool.quality,
                            newTool.GetDataToSave()));
                    }
                    _newBag.Add(newTool);
                }
                index++;
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
            int? count = null;
            ItemDescription selectedToolDescription =
                _itemDatabase.GetItemData(_newBag[_selectedIndex].ItemIndex);
            if(selectedToolDescription.ToolType == ToolType.SeedPlacer)
            {
                count = _toolsBagInventory.GetItemDataAt(_selectedIndex - 1).count;
            }
            List<Sprite> sprites = new List<Sprite>();
            foreach (Tool tool in _newBag)
            {
                ItemDescription toolDescription = _itemDatabase.GetItemData(tool.ItemIndex);
                if(toolDescription != null)
                {
                    sprites.Add(toolDescription.Image);
                }
            }
            OnToolsBagUpdated?.Invoke(_selectedIndex, sprites, count);
        }

        private void EquipTool(IAgent agent)
        {
            _newBag[_selectedIndex].Equip(agent);
            _newBag[_selectedIndex].OnFinishedAction += UpdateInventoryData;
        }

        private void UpdateInventoryData(IAgent agent)
        {
            Tool tool = _newBag[_selectedIndex];
            string data = tool.GetDataToSave();
            if (string.IsNullOrEmpty(data))
                return;
            int inventoryIndex = _selectedIndex - 1; // Hand is NOT in the inventory
            if(inventoryIndex >= 0)
            {
                if (tool.IsToolStillValid())
                {
                    //modified the item
                    int quantity = 1;
                    if(tool is IQuantity)
                    {
                        quantity = ((IQuantity)tool).Quantity;
                    }
                    _toolsBagInventory.AddItemAt(inventoryIndex
                        , new InventoryItemData(tool.ItemIndex, quantity, -1, data));
                }
                else
                {
                    //recreate the inventory
                    List<InventoryItemData> items = _toolsBagInventory.InventoryContent.ToList();
                    _toolsBagInventory.Clear();
                    for (int i = 0; i < items.Count; i++) 
                    { 
                        if(i == inventoryIndex || items[i] == null)
                        {
                            continue;
                        }
                        _toolsBagInventory.AddItem(items[i],
                            _itemDatabase.GetItemData(items[i].id).StackQuantity);
                    }
                    UpdateToolsBag(_toolsBagInventory.InventoryContent);
                }
                SwapTool(_selectedIndex, agent);
            }
            
        }

        private void PutAway(IAgent agent)
        {
            _newBag[_selectedIndex].PutAway(agent);
            _newBag[_selectedIndex].OnFinishedAction = null;
            _newBag[_selectedIndex].OnPerformedAction = null;
            _newBag[_selectedIndex].OnStartedAction = null;
        }
    }
}
