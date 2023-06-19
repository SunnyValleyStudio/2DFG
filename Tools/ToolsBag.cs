using FarmGame.Agent;
using FarmGame.DataStorage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class ToolsBag : MonoBehaviour
    {
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        private int _selectedIndex = 0;

        [SerializeField]
        private List<int> _initialTools;

        private Tool _currentTool;

        public Tool CurrentTool => _currentTool;

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
            if(_currentTool != null)
            {
                PutAway(agent);
            }
            _selectedIndex = newIndex;
            if(_selectedIndex >= _initialTools.Count)
            {
                _selectedIndex = 0;
            }
            ItemDescription description = _itemDatabase.GetItemData(_initialTools[_selectedIndex]);
            Debug.Log($" Equipped tool: {description.Name}");
            _currentTool = ToolFactory.CreateTool(description);
            EquipTool(agent);
        }

        private void EquipTool(IAgent agent)
        {
            _currentTool.Equip(agent);
        }

        private void PutAway(IAgent agent)
        {
            _currentTool.PutAway(agent);
        }
    }
}
