using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FarmGame
{
    public class AgentMover : MonoBehaviour
    {
        public Vector2 MovementInput { get; set; }
        public Transform _testTransform;

        private void Update()
        {
            _testTransform.position += (Vector3)MovementInput * Time.deltaTime;
        }
    }
}
