using FarmGame.Agent;
using FarmGame.Interactions;
using FarmGame.SellSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class ToolBoxInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private SellBoxController _toolBoxController;
        public List<ToolType> UsableTools { get; set; } = new() { ToolType.Hand };

        public bool CanInteract(IAgent agent)
        => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent)
        {
            Debug.Log("Interacting with ToolBox");
            _toolBoxController.PrepareSellBox(agent.ToolsBag.GetInventory());
        }
    }
}
