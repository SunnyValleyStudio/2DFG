using FarmGame.Agent;
using FarmGame.Input;
using FarmGame.TimeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FarmGame.DialogueSystem
{
    public class DialogueControllerUI : MonoBehaviour
    {
        public UnityEvent OnShowDialogueWindow, OnHideDialogueWindow;

        [SerializeField]
        private PlayerInputFarm _input;

        [SerializeField]
        private Image _portraitImage;
        private TextMeshProUGUI _nameTag, _dialogueText;

        [SerializeField]
        private PauseTimeControllerSO _pauseTimeController;

        Player _playerReference;
        public event Action OnShowUI, OnHideUI;

        private Action _progressDialogueAction;

        public void PrepareDialogueUI(DialogueData dialogueData)
        {
            _input.EnableUIActionMap();
            _input.OnUIExit += ExitUI;

            _pauseTimeController.SetTimePause(true);

            ShowDialogueUIData(dialogueData);
        }

        private void ShowDialogueUIData(DialogueData dialogueData)
        {
            SetDialogueUIData(dialogueData);
            _progressDialogueAction = () => ProgressDialogue(dialogueData);
            _input.OnUIInteract += _progressDialogueAction;
            gameObject.SetActive(true);
            OnShowUI?.Invoke();
        }

        private void ProgressDialogue(DialogueData dialogueData)
        {
            if(_progressDialogueAction != null)
            {
                _input.OnUIInteract -= _progressDialogueAction;
                _progressDialogueAction = null;
            }
            if(dialogueData.NextDialogue != null)
            {
                ShowDialogueUIData(dialogueData.NextDialogue);
            }
            else
            {
                ExitUI();
            }
            dialogueData.OnActon?.Invoke(_playerReference);
        }

        private void SetDialogueUIData(DialogueData dialogueData)
        {
            if(dialogueData.CharacterData != null)
            {
                _portraitImage.sprite = dialogueData.CharacterData.Portrait;
                _nameTag.text = dialogueData.CharacterData.CharacterName;
            }
            _dialogueText.text = dialogueData.Text;
        }

        private void ExitUI()
        {
            _pauseTimeController.SetTimePause(false);
            _input.EnableDefaultActionMap();
            _input.OnUIExit -= ExitUI;
            if (_progressDialogueAction != null)
            {
                _input.OnUIInteract -= _progressDialogueAction;
                _progressDialogueAction = null;
            }
            gameObject.SetActive(false);
            OnHideDialogueWindow?.Invoke();
            OnHideUI?.Invoke();
            OnHideUI = null;
            OnShowUI = null;
        }
    }
}
