using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FarmGame.Agent
{
    public class AgentMover : MonoBehaviour
    {
        public Vector2 MovementInput { get; private set; }

        [SerializeField]
        private Rigidbody2D _rigidbody;

        public event Action<bool> OnMove;

        [SerializeField]
        private float _speed = 2;

        private void FixedUpdate()
        {
            Vector2 velocity = MovementInput * _speed;
            _rigidbody.MovePosition(
                _rigidbody.position + velocity * Time.fixedDeltaTime);
        }

        internal void SetMovementInput(Vector2 input)
        {
            MovementInput = input;
            OnMove?.Invoke(input.magnitude > 0.1f);
        }
    }
}
