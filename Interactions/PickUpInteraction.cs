using FarmGame.Agent;
using FarmGame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Interactions
{
    public class PickUpInteraction : MonoBehaviour
    {
        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } 
            = new List<ToolType>();

        public bool CanInteract(Player agent)
            => UsableTools.Contains(agent.SelectedTool.ToolType);

        public void Interact(Player agent)
        {
            Destroy(gameObject);
        }
    }
}
