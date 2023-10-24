using FarmGame.Agent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.DialogueSystem
{
    public class DialogueData : MonoBehaviour
    {
        [field: SerializeField]
        public CharacterDataSO CharacterData { get; private set; }
        [field: SerializeField, Multiline]
        public string Text { get; set; }

        public UnityEvent<IAgent> OnActon;

        [field: SerializeField]
        public DialogueData NextDialogue { get; private set; }


    }
}
