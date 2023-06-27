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
    }

    public record InventoryItemData(int id, int count, int quality, string data = null);
}

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit
    {

    }
}