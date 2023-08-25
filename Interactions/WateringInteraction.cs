using FarmGame.Agent;
using FarmGame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Interactions
{
    public class WateringInteraction : MonoBehaviour, IInteractable
    {
        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } = new() { ToolType.WatringCan};

        public bool CanInteract(IAgent agent)
        => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent)
        {
            //agent.FieldController.WaterCropAt(transform.position);
        }
    }
}
