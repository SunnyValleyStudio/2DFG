using FarmGame.Agent;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarmGame.Tools
{
    public class ToolsBag : MonoBehaviour, ISavable
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

        public int SaveID => SaveIDRepositor.TOOLS_BAG_ID;

        public event Action<int, List<Sprite>, int?> OnToolsBagUpdated;

        private void Awake()
        {
            _toolsBagInventory.OnUpdateInventory += (inventoryContent) =>
            {
                UpdateToolsBag(inventoryContent);
                SendUpdateMessage();
            };
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
                            newTool.GetDataToSave()), false);
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
            else if(selectedToolDescription.ToolType == ToolType.WatringCan)
            {
                count = ((WateringCanTool)CurrentTool).NumberOfUses;
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
            if (_selectedIndex != 0)
                _newBag[_selectedIndex].OnFinishedAction += (a) =>
                a.AgentStaminaSystem.ModifyStamina(_newBag[_selectedIndex].ToolType);
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

        public void RestoreCurrentTool(IAgent agent)
        {
            if(CurrentTool.ToolType == ToolType.WatringCan)
            {
                ((WateringCanTool)CurrentTool).Refill();
            }
            UpdateInventoryData(agent);
        }

        public string GetData()
        {
            ToolsBadSaveData data = new()
            {
                selectedToolIndex = _selectedIndex,
                toolsInventoryData = _toolsBagInventory.GetDataToSave()
            };
            return JsonUtility.ToJson(data);
        }

        public void RestoreData(string data)
        {
            _toolsBagInventory.OnUpdateInventory += UpdateToolsBag;
            if (string.IsNullOrEmpty(data))
            {
                for (int i = 0; i < _initialTools.Count; i++)
                {
                    ItemDescription description = _itemDatabase.GetItemData(_initialTools[i]);
                    string toolData = null;
                    int quantity = 1;
                    if (description.ToolType == ToolType.SeedPlacer)
                    {
                        toolData = JsonUtility.ToJson(new SeedToolData
                        {
                            cropID = description.CropTypeIndex,
                            quantity = 2
                        });
                        quantity = 2;
                    }
                    _toolsBagInventory.AddItem(new InventoryItemData(description.ID, quantity,
                        -1, toolData), description.StackQuantity, false);
                }
                UpdateToolsBag(_toolsBagInventory.InventoryContent);
                return;
            }
            ToolsBadSaveData loadedData = JsonUtility.FromJson<ToolsBadSaveData>(data);
            _selectedIndex = loadedData.selectedToolIndex;
            _toolsBagInventory.RestoreSavedData(loadedData.toolsInventoryData);
        }

        internal Inventory GetInventory()
        => _toolsBagInventory;

        [Serializable]
        public struct ToolsBadSaveData
        {
            public int selectedToolIndex;
            public string toolsInventoryData;
        }

    }
}
