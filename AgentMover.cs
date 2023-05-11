using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;

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

        [SerializeField]
        private CollisionDetector _collisionDetector;

        private bool _stopped;

        public bool Stopped
        {
            get { return _stopped; }
            set { _stopped = value; }
        }

        private void FixedUpdate()
        {
            if (_stopped) return;

            Vector2 velocity = MovementInput * _speed;

            float distanceToMoveThisFrame = velocity.magnitude * Time.fixedDeltaTime;

            if(_collisionDetector.IsMovementValid(MovementInput,distanceToMoveThisFrame) == false)
            {
                velocity = Vector2.zero;
            }

            OnMove?.Invoke(velocity.magnitude > 0.1f);

            _rigidbody.MovePosition(
                _rigidbody.position + velocity * Time.fixedDeltaTime);
        }

        public void SetMovementInput(Vector2 input)
        {
            MovementInput = input;
        }
    }
}
