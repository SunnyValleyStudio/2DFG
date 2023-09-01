using FarmGame.Agent;
using FarmGame.SellSystem;
using FarmGame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Interactions
{
    public class SellBoxInteraction : MonoBehaviour, IInteractable
    {
        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } = new() { ToolType.Hand };

        [SerializeField]
        private SellBoxController _sellBoxController;
        public bool CanInteract(IAgent agent)
        => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent)
        {
            _sellBoxController.PrepareSellBox(agent.Inventory);
        }
    }
}
