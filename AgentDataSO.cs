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

	}
}
