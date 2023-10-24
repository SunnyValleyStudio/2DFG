using FarmGame.Agent;
using FarmGame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Interactions
{
    public class NPCObjectDialogueInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private NPCDialogueInteracton _npcInteraction;
        public List<ToolType> UsableTools { get => _npcInteraction.UsableTools; set { _npcInteraction.UsableTools = value; } }

        public bool CanInteract(IAgent agent)
        {
            return _npcInteraction.CanInteract(agent);
        }

        public void Interact(IAgent agent)
        {
            _npcInteraction.Interact(agent);
        }
    }
}
