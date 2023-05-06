using FarmGame.Input;
using FarmGame.Interactions;
using FarmGame.Tools;
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

        private HandTool _selectedTool = new HandTool(ToolType.Hand);

        public AgentMover AgentMover
        {
            get => _agentMover;
        }
        public PlayerInputFarm AgentInput
        {
            get => _agentInput;
        }
        public AgentAnimation AgentAnimation
        {
            get => _agentAnimation;
        }
        public InteractionDetector InteractionDetector
        { 
            get => _interactionDetector; 
        }

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
            _selectedTool.UseTool(this);
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
