using FarmGame.Agent;
using FarmGame.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class ToolBoxInteraction : MonoBehaviour, IInteractable
    {
        public List<ToolType> UsableTools { get; set; } = new() { ToolType.Hand };

        public bool CanInteract(IAgent agent)
        => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent)
        {
            Debug.Log("Interacting with ToolBox");
        }
    }
}
