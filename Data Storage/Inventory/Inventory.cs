using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FarmGame.DataStorage.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private InventoryItemData[] _inventoryContent;
        public IEnumerable<InventoryItemData> InventoryContent => _inventoryContent;
        [SerializeField]
        private int _capacity = 8;
        public int Capacity { get => _capacity; }

        public event Action<IEnumerable<InventoryItemData>> OnUpdateInventory;

        private void Awake()
        {
            if(_inventoryContent == null)
                _inventoryContent = new InventoryItemData[Capacity];
        }

        public void ChangeCapacity(int newCapacity)
        {
            if (newCapacity <= 0)
                return;
            InventoryItemData[] newInventoryContent = new InventoryItemData[newCapacity];
            for (int i = 0; i < _capacity; i++)
            {
                if (i >= newCapacity)
                    break;
                InventoryItemData item = _inventoryContent[i];
                if (item == null)
                    continue;
                newInventoryContent[i] 
                    = new InventoryItemData(item.id, item.count, item.quality, item.data);
            }
            _inventoryContent = newInventoryContent;
            _capacity = newCapacity;
        }

        public int AddItem(InventoryItemData item, int stackSize)
        {
            int quantityRemaining = item.count;
            if(stackSize > 1) 
            {
                for (int i = 0; i < _capacity; i++)
                {
                    if (_inventoryContent[i] != null 
                        && item.id == _inventoryContent[i].id
                        && _inventoryContent[i].quality == item.quality)
                    {
                        int freeSpace = stackSize = _inventoryContent[i].count;
                        int quantityToStore = quantityRemaining;
                        if(quantityRemaining > freeSpace)
                        {
                            quantityToStore = freeSpace;
                            quantityRemaining -= freeSpace; ;
                        }
                        else
                        {
                            quantityRemaining = 0;
                        }

                        _inventoryContent[i]
                            = new InventoryItemData(item.id, _inventoryContent[i].count + quantityToStore, item.quality,
                            item.data);

                        OnUpdateInventory?.Invoke(InventoryContent);
                        if (quantityRemaining <= 0)
                            return 0;
                    }
                }
            }
            if (quantityRemaining > 0)
            {
                for (int i = 0; i < _capacity; i++)
                {
                    if (_inventoryContent[i] == null)
                    {
                        int quantityToAdd;
                        if(stackSize > 1)
                        {
                            quantityToAdd = quantityRemaining > stackSize ? stackSize : quantityRemaining;
                        }
                        else
                        {
                            quantityToAdd = 1;
                        }
                        _inventoryContent[i] = new InventoryItemData(item.id, quantityToAdd, item.quality, item.data);
                        quantityRemaining -= quantityToAdd;

                        OnUpdateInventory?.Invoke(InventoryContent);
                        if(quantityRemaining <= 0) 
                            return 0;
                    }
                }
            }
            return quantityRemaining;
        }

        public bool IsThereSpace(InventoryItemData item, int stackSize)
        {
            if(stackSize > 1)
            {
                return _inventoryContent.Any(data => data == null)
                    || _inventoryContent.Where(
                        data => data.id == item.id && data.count + item.count <= stackSize)
                    .Count() > 0;
            }
            return _inventoryContent.Any(data => data == null);
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("Inventory content: ");
            foreach (var item in InventoryContent)
            {
                if (item == null)
                    sb.Append("NULL, ");
                else
                    sb.Append(item.id + $"({item.count}), ");
            }
            return sb.ToString();
        }

        public InventoryItemData GetItemDataAt(int index)
        {
            if(index >= _capacity || index < 0)
            {
                return null;
            }
            return _inventoryContent[index];
        }

        public bool SetItemDataAt(int index, InventoryItemData item)
        {
            if (index >= _capacity || index < 0)
            {
                return false;
            }
            _inventoryContent[index] = item;
            OnUpdateInventory?.Invoke(InventoryContent);
            return true;
        }

        public void RemoveAllItem(InventoryItemData item)
        {
            int index = Array.IndexOf(_inventoryContent, item);
            if(index > -1)
            {
                _inventoryContent[index] = null;
                OnUpdateInventory?.Invoke(InventoryContent);
            }
        }

        public bool RemoveAllItemAt(int index)
        {
            if (index >= _capacity || index < 0)
            {
                return false;
            }
            _inventoryContent[index] = null;
            OnUpdateInventory?.Invoke(InventoryContent);
            return true;
        }

        public bool AddItemAt(int index, InventoryItemData item)
        {
            if(index >= _capacity || index < 0)
            {
                return false;
            }
            _inventoryContent[index] = item;
            OnUpdateInventory?.Invoke(InventoryContent);
            return true; 
        }
    }

    public record InventoryItemData(int id, int count, int quality, string data = null);
}

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit
    {

    }
}