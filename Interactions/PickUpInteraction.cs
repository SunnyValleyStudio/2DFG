using FarmGame.Agent;
using FarmGame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Interactions
{
    public class PickUpInteraction : MonoBehaviour, IInteractable
    {
        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; }
            = new List<ToolType>();

        public UnityEvent OnPickUp;

        public bool CanInteract(Player agent)
            => UsableTools.Contains(agent.SelectedTool.ToolType);

        public void Interact(Player agent)
        {
            OnPickUp?.Invoke();
            Destroy(gameObject);
        }
    }
}
