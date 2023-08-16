using FarmGame.Agent;
using FarmGame.Tools;
using FarmGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Interactions
{
    public class SleepInteraction : MonoBehaviour, IInteractable
    {
        public List<ToolType> UsableTools { get; set; } 
            = new() { ToolType.Hand };

        public UnityEvent OnAfterFinishedSleeping, OnMoveToNextDay;

        public bool CanInteract(IAgent agent)
            => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        [SerializeField]
        private ScreenTransitionEffect _transitionEffeect;

        private void Awake()
        {
            _transitionEffeect = FindObjectOfType<ScreenTransitionEffect>(true);
        }

        public void Interact(IAgent agent)
        {
            Debug.Log("Going To Sleep");
            StartCoroutine(SleepTransition(agent));
        }

        private IEnumerator SleepTransition(IAgent agent)
        {
            if(_transitionEffeect != null)
            {
                _transitionEffeect.PlayTransition(false);
            }
            agent.Blocked = true;
            yield return new WaitForSecondsRealtime(1);
            OnMoveToNextDay?.Invoke();
            if (_transitionEffeect != null)
            {
                _transitionEffeect.PlayTransition(true);
            }
            yield return new WaitForSecondsRealtime(1);
            agent.Blocked = false;
            OnAfterFinishedSleeping?.Invoke();
            Debug.Log("Finished Sleeping");
        }
    }
}
