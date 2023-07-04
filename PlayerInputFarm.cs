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
        public event Action OnPerformAction, OnSwapTool, OnToggleInventory;

        private void OnEnable()
        {
            _input.actions["Player/Movement"].performed += Move;
            _input.actions["Player/Movement"].canceled += Move;
            _input.actions["Player/Interact"].performed += Interact;
            _input.actions["Player/SwapTool"].performed += SwapTool;
            _input.actions["Player/ToggleInventory"].performed += ToggleInventory;
        }

        private void ToggleInventory(InputAction.CallbackContext obj)
        {
            OnToggleInventory?.Invoke();
        }

        private void SwapTool(InputAction.CallbackContext obj)
        {
            OnSwapTool?.Invoke();
        }

        public void BlockInput(bool val)
        {
            if (val)
            {
                Debug.LogWarning("Input has been BLOCKED", gameObject);
                _input.enabled = false;
            }
            else
            {
                Debug.LogWarning("Input has been unblocked", gameObject);
                _input.enabled = true;
            }
        }

        private void Interact(InputAction.CallbackContext obj)
        {
            OnPerformAction?.Invoke();
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
            _input.actions["Player/Interact"].performed -= Interact;
            _input.actions["Player/SwapTool"].performed -= SwapTool;
            _input.actions["Player/ToggleInventory"].performed -= ToggleInventory;
        }
    }
}
