using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FarmGame.Input
{
    public class PlayerInputFarm : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput _input;
        [field : SerializeField]
        public Vector2 InputValue { get; private set; }
        public UnityEvent<Vector2> OnMoveInput;


        private void OnEnable()
        {
            _input.actions["Player/Movement"].performed += Move;
            _input.actions["Player/Movement"].canceled += Move;
        }

        private void Move(InputAction.CallbackContext obj)
        {
            InputValue = obj.ReadValue<Vector2>();
            OnMoveInput?.Invoke(InputValue);
        }



        private void OnDisable()
        {
            _input.actions["Player/Movement"].performed -= Move;
            _input.actions["Player/Movement"].canceled -= Move;
        }
    }
}
