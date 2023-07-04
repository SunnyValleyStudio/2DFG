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
        public event Action OnUIExit, OnUIToggleInventory, OnUIInteract;
        public event Action<Vector2> OnUIMoveInput;

        private void OnEnable()
        {
            _input.actions["Player/Movement"].performed += Move;
            _input.actions["Player/Movement"].canceled += Move;
            _input.actions["Player/Interact"].performed += Interact;
            _input.actions["Player/SwapTool"].performed += SwapTool;
            _input.actions["Player/ToggleInventory"].performed += ToggleInventory;

            _input.actions["UI/Movement"].performed += MoveUI;
            _input.actions["UI/Interact"].performed += InteractUI;
            _input.actions["UI/Exit"].performed += ExitUI;
            _input.actions["UI/ToggleInventory"].performed += ToggleInventoryUI;
        }

        private void ExitUI(InputAction.CallbackContext obj)
        {
            OnUIExit?.Invoke();
        }

        private void ToggleInventoryUI(InputAction.CallbackContext obj)
        {
            OnUIToggleInventory?.Invoke();
        }

        private void InteractUI(InputAction.CallbackContext obj)
        {
            OnUIInteract?.Invoke();
        }

        private void MoveUI(InputAction.CallbackContext obj)
        {
            Vector2 input = obj.ReadValue<Vector2>();
            OnUIMoveInput?.Invoke(input);
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

        public void EnableUIActionMap()
        {
            _input.SwitchCurrentActionMap("UI");
        }

        public void EnableDefaultActionMap()
        {
            _input.SwitchCurrentActionMap("Player");
        }


        private void OnDisable()
        {
            _input.actions["Player/Movement"].performed -= Move;
            _input.actions["Player/Movement"].canceled -= Move;
            _input.actions["Player/Interact"].performed -= Interact;
            _input.actions["Player/SwapTool"].performed -= SwapTool;
            _input.actions["Player/ToggleInventory"].performed -= ToggleInventory;

            _input.actions["UI/Movement"].performed -= MoveUI;
            _input.actions["UI/Interact"].performed -= InteractUI;
            _input.actions["UI/Exit"].performed -= ExitUI;
            _input.actions["UI/ToggleInventory"].performed -= ToggleInventoryUI;
        }
    }
}
