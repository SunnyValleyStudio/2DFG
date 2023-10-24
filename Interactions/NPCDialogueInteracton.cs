using FarmGame.Agent;
using FarmGame.DialogueSystem;
using FarmGame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Interactions
{
    public class NPCDialogueInteracton : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private DialogueControllerUI _dialogueController;
        [SerializeField]
        private DialogueData _dialogueData;

        public List<ToolType> UsableTools { get; set; } = new() { ToolType.Hand };

        private void Awake()
        {
            _dialogueController = FindObjectOfType<DialogueControllerUI>(true);
        }
        public bool CanInteract(IAgent agent)
        {
            if(agent is not Player)
                return false;
            return UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);
        }

        public void Interact(IAgent agent)
        {
            _dialogueController.PrepareDialogueUI(_dialogueData);
        }
    }
}
