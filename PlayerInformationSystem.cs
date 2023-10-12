using FarmGame.Agent;
using FarmGame.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame
{
    public class PlayerInformationSystem : MonoBehaviour
    {
        [SerializeField]
        private DialogueControllerUI _dialogueUI;
        [SerializeField]
        private DialogueData _dialogueData;

        public void ShowInformation(Player player, string infoText)
        {
            _dialogueData.Text = infoText;
            _dialogueUI.OnShowUI += () => player.SetUIVisibility(false);
            _dialogueUI.OnHideUI += () => player.SetUIVisibility(true);
            _dialogueUI.PrepareDialogueUI(_dialogueData);
        }
    }
}
