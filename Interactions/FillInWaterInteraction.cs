using FarmGame.Agent;
using FarmGame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Interactions
{
    public class FillInWaterInteraction : MonoBehaviour, IInteractable
    {
        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } = new() { ToolType.WatringCan };

        public UnityEvent OnInteract;
        public bool CanInteract(IAgent agent)
        => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent)
        {
            agent.ToolsBag.RestoreCurrentTool(agent);
            OnInteract?.Invoke();
        }
    }
}
