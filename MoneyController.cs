using FarmGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Agent
{
    public class MoneyController : MonoBehaviour
    {
        [SerializeField]
        private AgentDataSO _agentData;
        [SerializeField]
        private MoneyUI _moneyUI;

        private void OnEnable()
        {
            if (_moneyUI == null || _agentData == null)
                return;
            _agentData.OnDataUpdated += UpdateMoneyUI;
        }

        private void Start()
        {
            if (_moneyUI == null || _agentData == null)
                return;
            UpdateMoneyUI(_agentData);
        }

        private void UpdateMoneyUI(AgentDataSO agentData)
        {
            _moneyUI.UpdateMoneyValue(agentData.Money);
        }
        private void OnDisable()
        {
            if (_moneyUI == null || _agentData == null)
                return;
            _agentData.OnDataUpdated -= UpdateMoneyUI;
        }
    }
}
