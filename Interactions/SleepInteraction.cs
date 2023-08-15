using FarmGame.Agent;
using FarmGame.Tools;
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

        public void Interact(IAgent agent)
        {
            Debug.Log("Going To Sleep");
            StartCoroutine(SleepTransition(agent));
        }

        private IEnumerator SleepTransition(IAgent agent)
        {
            agent.Blocked = true;
            yield return new WaitForSecondsRealtime(1);
            OnMoveToNextDay?.Invoke();
            yield return new WaitForSecondsRealtime(1);
            agent.Blocked = false;
            OnAfterFinishedSleeping?.Invoke();
            Debug.Log("Finished Sleeping");
        }
    }
}
