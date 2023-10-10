using FarmGame.DataStorage.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Agent
{
    [CreateAssetMenu]
    public class AgentDataSO : ScriptableObject
    {
		[SerializeField]
		private int _money;

		public event Action<AgentDataSO> OnDataUpdated;
		public int Money
		{
			get { return _money; }
			set { 
				_money = value; 
				OnDataUpdated?.Invoke(this);
			}
		}
        [field: SerializeField]
        public int StaminaMax { get; private set; } = 100;
        [SerializeField]
        private int _currentStamina;

        public int CurrentStamina
        {
            get { return _currentStamina; }
            set { 
                _currentStamina = value; 
                OnDataUpdated?.Invoke(this);
            }
        }


        public Inventory Inventory { get; internal set; }

        internal string GetData()
        {
            AgentSaveData data = new()
            {
                money = _money,
                currentStamina = _currentStamina,
                maxStamina = StaminaMax,
                inventoryData = Inventory.GetDataToSave()
            };
            return JsonUtility.ToJson(data);
        }

        internal void SetDefaultData()
        {
            Money = 0;
            CurrentStamina = StaminaMax;
        }

        internal void RestoreData(string data)
        {
            AgentSaveData loadedData = JsonUtility.FromJson<AgentSaveData>(data);
            Money = loadedData.money;
            CurrentStamina = loadedData.currentStamina;
            StaminaMax = loadedData.maxStamina;
            Inventory.RestoreSavedData(loadedData.inventoryData);
        }

        [Serializable]
        public struct AgentSaveData
        {
            public int money, currentStamina, maxStamina;
            public string inventoryData;
        }
    }
}
