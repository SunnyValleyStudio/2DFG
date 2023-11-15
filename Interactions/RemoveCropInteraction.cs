using FarmGame.Agent;
using FarmGame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Interactions
{
    public class RemoveCropInteraction : MonoBehaviour, IInteractable
    {
        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } = new() { ToolType.Hoe };

        public bool CanInteract(IAgent agent)
        {
            return UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);
        }

        public void Interact(IAgent agent)
        {
            agent.FieldController.RemoveCropAt(transform.position);
        }
    }
}
