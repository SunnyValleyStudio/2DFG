using FarmGame.Agent;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.TimeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarmGame.SellSystem
{
    [RequireComponent(typeof(Inventory))]
    public class SellManager : MonoBehaviour
    {
        [SerializeField, Range(0, 23)]
        private int _sellHour = 12;

        private bool _readyToSell = true;
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;
        [SerializeField]
        private AgentDataSO _playerAgentData;

        private TimeManager _timeManager;

        private Inventory _sellBoxInventory;

        private void Awake()
        {
            _sellBoxInventory = GetComponent<Inventory>();
            _timeManager = FindObjectOfType<TimeManager>(true);
            if(_timeManager == null)
            {
                Debug.LogWarning("Can't find TimeManager", gameObject);
                return;
            }
            _timeManager.OnClockProgress += TrySellingItems;
            _timeManager.OnDayProgress += ResetSellSystem;
        }

        private void ResetSellSystem(object sender, TimeEventArgs e)
        {
            if(e.TheSameDay)
            {
                return;
            }
            if (_readyToSell)
            {
                PerformSelling();
            }
            _readyToSell = true;
            Debug.Log("Resetting the Sell System");
        }

        private void PerformSelling()
        {
            _readyToSell = false;
            Debug.Log("Selling Items");
            foreach (var item in _sellBoxInventory.InventoryContent)
            {
                if (item == null)
                    continue;
                ItemDescription description = _itemDatabase.GetItemData(item.id);
                _playerAgentData.Money += description.Price * item.count;
            }
            _sellBoxInventory.Clear();
        }

        private void TrySellingItems(object sender, TimeEventArgs e)
        {
            if(e.CurrentTime.Hours == _sellHour && e.CurrentTime.Minutes == 0)
            {
                PerformSelling();
            }
        }
    }
}
