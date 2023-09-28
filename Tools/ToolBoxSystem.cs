using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class ToolBoxSystem : MonoBehaviour, ISavable
    {
        public int SaveID => SaveIDRepositor.TOOL_BOX_ID;

        [SerializeField]
        private Inventory _toolBoxInventory;

        [SerializeField]
        private List<InitialToolData> _initialTools;

        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        public string GetData()
        {
            return _toolBoxInventory.GetDataToSave();
        }

        public void RestoreData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                foreach (var item in _initialTools)
                {
                    if (item == null)
                        continue;
                    ItemDescription description = _itemDatabase.GetItemData(item.ID);
                    InventoryItemData itemData = 
                        new InventoryItemData(item.ID, item.amount, -1
                        , ToolFactory.GetToolData(description,item.amount));

                    _toolBoxInventory.AddItem(itemData, description.StackQuantity);
                }
            }
            else
            {
                _toolBoxInventory.RestoreSavedData(data);
            }
        }

        [Serializable]
        public class InitialToolData
        {
            public int ID;
            public int amount = 1;
        }
    }
}
