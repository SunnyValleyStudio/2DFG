using FarmGame.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.UI
{
    [RequireComponent(typeof(InventoryRendererUI))]
    public class ItemSelectionUI : MonoBehaviour
    {
        [SerializeField]
        private int _selectedItemIndex = 0;
        private InventoryRendererUI _inventoryRenderer;
        public UnityEvent<Direction, int> OnSelectOutsideOfBounds;
        public UnityEvent<int> OnSelectInsideChange;
        public int SelectedItem => _selectedItemIndex;

        private void Awake()
        {
            _inventoryRenderer = GetComponent<InventoryRendererUI>();
        }

        public void EnableController(PlayerInputFarm myInput)
        {
            myInput.OnUIMoveInput += SelectItem;
            if (_inventoryRenderer == null)
                Awake();
            _inventoryRenderer.SelectItem(_selectedItemIndex);
            SelectItem(Vector2.zero);
        }

        public void DisableController(PlayerInputFarm myInput)
        {
            myInput.OnUIMoveInput -= SelectItem;
        }

        private void SelectItem(Vector2 playerInput)
        {
            Vector2Int input = Vector2Int.RoundToInt(playerInput);
            int newIndex = 0;
            Direction direction = Direction.None;
            (newIndex, direction) = FindDirection(input);

            int currentRow = _selectedItemIndex / _inventoryRenderer.RowSize;
            int newRow = newIndex / _inventoryRenderer.RowSize;
            int currentColumn = _selectedItemIndex % _inventoryRenderer.RowSize;
            int newColumn = newIndex % _inventoryRenderer.RowSize;

            if(newIndex > -1 && newIndex < _inventoryRenderer.InventoryItemCount 
                && (newRow == currentRow || newColumn == currentColumn))
            {
                SeletItemAt(newIndex);
            }
            else
            {
                OnSelectOutsideOfBounds?.Invoke(direction, newIndex);
            }
        }

        private void SeletItemAt(int newIndex)
        {
            _inventoryRenderer.ResetAllSelection(false);
            _selectedItemIndex = newIndex;
            _inventoryRenderer.SelectItem(_selectedItemIndex);
            OnSelectInsideChange?.Invoke(_selectedItemIndex);
        }

        private (int,Direction) FindDirection(Vector2Int input)
        {
            int newIndex = 0;
            Direction direction = Direction.None;
            if (input.x == 1)
            {
                newIndex = _selectedItemIndex + 1;
                direction = Direction.Right;
            }
            else if (input.x == -1)
            {
                newIndex = _selectedItemIndex - 1;
                direction = Direction.Left;
            }
            else if (input.y == 1)
            {
                newIndex = _selectedItemIndex - _inventoryRenderer.RowSize;
                direction = Direction.Up;
            }
            else if (input.y == -1)
            {
                newIndex = _selectedItemIndex + _inventoryRenderer.RowSize;
                direction = Direction.Down;
            }
            return (newIndex, direction);
        }

        public void WrapHorizontalMovementSelecton(Direction direction, int index)
        {
            if(direction == Direction.Left)
            {
                int wrappedIndex = _selectedItemIndex + _inventoryRenderer.RowSize - 1;
                int currentRow = _selectedItemIndex / _inventoryRenderer.RowSize;
                int newRow = wrappedIndex / _inventoryRenderer.RowSize;
                if(wrappedIndex >= _inventoryRenderer.InventoryItemCount || newRow != currentRow)
                {
                    return;
                }
                _inventoryRenderer.ResetAllSelection(false);
                _selectedItemIndex = wrappedIndex;
                _inventoryRenderer.SelectItem(_selectedItemIndex);
                OnSelectInsideChange?.Invoke(_selectedItemIndex);
            }
            if(direction == Direction.Right)
            {
                int wrappedIndex = _selectedItemIndex - _inventoryRenderer.RowSize + 1;
                int currentRow = _selectedItemIndex / _inventoryRenderer.RowSize;
                int newRow = wrappedIndex / _inventoryRenderer.RowSize;
                if(wrappedIndex < 0 || newRow != currentRow)
                {
                    return;
                }
                _inventoryRenderer.ResetAllSelection(false);
                _selectedItemIndex = wrappedIndex;
                _inventoryRenderer.SelectItem(_selectedItemIndex);
                OnSelectInsideChange?.Invoke(_selectedItemIndex);
            }
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }
}
