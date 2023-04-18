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
        public Transform _testTransform;
        public event Action<bool> OnMove;
        [SerializeField]
        private float speed = 2;

        private void Update()
        {
            _testTransform.position += (Vector3)MovementInput*speed * Time.deltaTime;
        }

        internal void SetMovementInput(Vector2 input)
        {
            MovementInput = input;
            OnMove?.Invoke(input.magnitude > 0.1f);
        }
    }
}
