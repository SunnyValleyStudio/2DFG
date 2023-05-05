using FarmGame.Input;
using FarmGame.Interactions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Agent
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private AgentMover _agentMover;
        [SerializeField]
        private PlayerInputFarm _agentInput;
        [SerializeField]
        private AgentAnimation _agentAnimation;

        [SerializeField]
        private InteractionDetector _interactionDetector;

        private void OnEnable()
        {
            _agentInput.OnMoveInput.AddListener(_agentMover.SetMovementInput);
            _agentInput.OnMoveInput.AddListener(_agentAnimation.ChangeDirection);
            _agentInput.OnMoveInput.AddListener(_interactionDetector.SetInteractionDirection);
            _agentInput.OnPerformAction += PerformAction;

            _agentMover.OnMove += _agentAnimation.PlayMovementAnimation;
        }

        private void PerformAction()
        {
            Debug.Log("Interacting");
            foreach (PickUpInteraction item in _interactionDetector.PerformDetection())
            {
                if (item.CanInteract())
                {
                    _agentMover.Stopped = true;
                    Debug.Log("Agent Stopped");
                    _agentAnimation.OnAnimationEnd.AddListener(
                        () =>
                        {
                            item.Interact(this);
                            _agentMover.Stopped = false;
                            Debug.Log("Agent Restarted");
                        }
                        );
                    _agentAnimation.PlayAnimation(AnimationType.PickUp);
                    return;
                }
            }
        }


        private void OnDisable()
        {
            _agentInput.OnMoveInput.RemoveListener(_agentMover.SetMovementInput);
            _agentInput.OnMoveInput.RemoveListener(_agentAnimation.ChangeDirection);
            _agentInput.OnMoveInput.RemoveListener(_interactionDetector.SetInteractionDirection);
            _agentInput.OnPerformAction -= PerformAction;

            _agentMover.OnMove -= _agentAnimation.PlayMovementAnimation;
        }
    }
}
