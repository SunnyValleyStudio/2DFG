using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FarmGame.UI
{
    public class GridLayoutScrollingUI : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _gridLayoutTransform;
        [SerializeField]
        private ScrollRect _scrollRect;
        [SerializeField]
        int _numberOfRows = 0;

        private bool _gridReady = false;
        private float _movementStep = 0;

        [SerializeField]
        private InventoryRendererUI _inventoryRenderer;

        private void PrepareScrolling()
        {
            DetectNumberOfRows();
            _movementStep = 1.0f / (_numberOfRows - 2.0f);
            _gridReady = true;
        }

        private void DetectNumberOfRows()
        {
            _numberOfRows 
                = _gridLayoutTransform.childCount / _inventoryRenderer.RowSize;
        }

        private Vector2Int GetGridPositonCoordinates(int index)
        {
            if(_gridReady == false)
            {
                PrepareScrolling();
            }
            return new Vector2Int(index % _inventoryRenderer.RowSize,
                Mathf.FloorToInt(index / _inventoryRenderer.RowSize));
        }

        public void OnSelectionChanged(int index)
        {
            if(_gridReady == false)
            {
                PrepareScrolling();
            }

            Vector2Int gridPos = GetGridPositonCoordinates(index);

            if(gridPos.y < 1)
            {
                _scrollRect.verticalNormalizedPosition = 1;
            }else if(gridPos.y > (_numberOfRows - 1)) 
            { 
                _scrollRect.verticalNormalizedPosition 
                    = Mathf.Clamp01(1 - _movementStep*(_numberOfRows - 1)); // 0
            }
            else
            {
                _scrollRect.verticalNormalizedPosition
                    = Mathf.Clamp01(1 - _movementStep * (gridPos.y - 1));
            }
        }
    }
}
