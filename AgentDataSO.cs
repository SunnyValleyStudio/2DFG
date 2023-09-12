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

        public Inventory Inventory { get; internal set; }

        internal string GetData()
        {
            AgentSaveData data = new()
            {
                money = _money,
                inventoryData = Inventory.GetDataToSave()
            };
            return JsonUtility.ToJson(data);
        }

        internal void SetDefaultData()
        {
            Money = 0;
        }

        internal void RestoreData(string data)
        {
            AgentSaveData loadedData = JsonUtility.FromJson<AgentSaveData>(data);
            Money = loadedData.money;
            Inventory.RestoreSavedData(loadedData.inventoryData);
        }

        [Serializable]
        public struct AgentSaveData
        {
            public int money;
            public string inventoryData;
        }
    }
}
