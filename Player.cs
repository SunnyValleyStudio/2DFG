using FarmGame.Input;
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

        private void OnEnable()
        {
            _agentInput.OnMoveInput.AddListener(_agentMover.SetMovementInput);
            _agentInput.OnMoveInput.AddListener(_agentAnimation.ChangeDirection);
            _agentMover.OnMove += _agentAnimation.PlayMovementAnimation;
        }
        private void OnDisable()
        {
            _agentInput.OnMoveInput.RemoveListener(_agentMover.SetMovementInput);
            _agentInput.OnMoveInput.RemoveListener(_agentAnimation.ChangeDirection);
            _agentMover.OnMove -= _agentAnimation.PlayMovementAnimation;
        }
    }
}
