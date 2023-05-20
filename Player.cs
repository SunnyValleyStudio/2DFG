using FarmGame.Farming;
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

        private Tool _selectedTool = new HandTool(ToolType.Hand);

        private bool _blocked = false;

        public bool Blocked
        {
            get { 
                return _blocked; 
            }
            set { 
                _blocked = value; 
                _agentMover.Stopped = _blocked;
                _agentInput.BlockInput(_blocked);
            }
        }
        [SerializeField]
        private FieldDetector _fieldDetector;

        public FieldDetector FieldDetectorObject
        {
            get { return _fieldDetector; }
        }

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
        public Tool SelectedTool 
        { 
            get => _selectedTool;
        }

        private void OnEnable()
        {
            _agentInput.OnMoveInput.AddListener(_agentMover.SetMovementInput);
            _agentInput.OnMoveInput.AddListener(_agentAnimation.ChangeDirection);
            _agentInput.OnMoveInput.AddListener(_interactionDetector.SetInteractionDirection);
            _agentInput.OnMoveInput.AddListener(_fieldDetector.SetInteractionDirection);
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
            _agentInput.OnMoveInput.RemoveListener(_fieldDetector.SetInteractionDirection);
            _agentInput.OnPerformAction -= PerformAction;

            _agentMover.OnMove -= _agentAnimation.PlayMovementAnimation;
        }
    }
}
