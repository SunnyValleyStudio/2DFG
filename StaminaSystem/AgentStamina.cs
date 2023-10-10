using FarmGame.Agent;
using FarmGame.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.StaminaSystem
{
    public class AgentStamina : MonoBehaviour
    {
        [SerializeField]
        private AgentDataSO _agentData;

        public UnityEvent<int, int> OnStaminaChange;
        public UnityEvent OnNotEnoughStamina, OnAgentFaint;

        private void OnEnable()
        {
            _agentData.OnDataUpdated += UpdateStaminaUI;
        }

        private void OnDisable()
        {
            _agentData.OnDataUpdated -= UpdateStaminaUI;
        }

        private void UpdateStaminaUI(AgentDataSO data)
        {
            OnStaminaChange?.Invoke(data.CurrentStamina, data.StaminaMax);
        }

        public bool IsThereEnoughStamina(int staminaCost = 1)
        {
            bool result = _agentData.CurrentStamina - staminaCost > -1;
            if(result == false)
            {
                Debug.Log("<color=red>Not enough Stamina!</color>");
                OnNotEnoughStamina?.Invoke();
            }
            if (_agentData.CurrentStamina == 0)
                OnAgentFaint?.Invoke();
            return result;
        }

        public void ReplenishStamina()
        {
            ModifyStamina(_agentData.StaminaMax);
        }

        public void ModifyStamina(int value)
        {
            _agentData.CurrentStamina = Mathf.Clamp(_agentData.CurrentStamina + value,
                0, _agentData.StaminaMax);
        }

        public void ModifyStamina(ToolType tool)
        {
            Debug.Log($"Modifying stamina based on tool {tool}");
            ModifyStamina(-1);
        }
    }

}
