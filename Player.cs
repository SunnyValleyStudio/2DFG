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

        private void OnEnable()
        {
            _agentInput.OnMoveInput.AddListener(_agentMover.SetMovementInput);
        }
        private void OnDisable()
        {
            _agentInput.OnMoveInput.RemoveListener(_agentMover.SetMovementInput);
        }
    }
}
