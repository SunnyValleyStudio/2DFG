using FarmGame.DataStorage;
using FarmGame.Interactions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FarmGame.Farming
{
    public class FieldRenderer : MonoBehaviour
    {
        [SerializeField]
        private Tilemap _preparedFieldTilemap;

        [SerializeField]
        private TileBase _preparedFieldTile;

        Dictionary<Vector3Int, GameObject> _cropVisualRepresentation = new();
        [SerializeField]
        private GameObject _cropPrefab;

        public Vector3Int GetTilemapTilePosition(Vector3 worldPosition)
            => _preparedFieldTilemap.WorldToCell(worldPosition);

        public void PrepareFieldAt(Vector3Int fieldCellPosition)
        {
            _preparedFieldTilemap.SetTile(fieldCellPosition,_preparedFieldTile);
        }

        public void CreateCropVisualization(Vector3Int tilePosition, Sprite cropSprite, 
            bool changeLayerOrder = false)
        {
            _cropVisualRepresentation[tilePosition] = Instantiate(_cropPrefab);
            _cropVisualRepresentation[tilePosition].transform.position = tilePosition 
                + new Vector3(0.5f,0.5f);
            UpdateCropVisualization(tilePosition, cropSprite, changeLayerOrder);
        }

        public void UpdateCropVisualization(Vector3Int tilePosition, Sprite cropSprite, bool changeLayerOrder)
        {
            CropRenderer renderer
                = _cropVisualRepresentation[tilePosition].GetComponent<CropRenderer>();
            renderer.SetSprite(cropSprite);
            if(changeLayerOrder)
            {
                renderer.ChangeLayerOrder();
            }
        }

        public void WiltCropVisualization(Vector3Int position)
        {
            if (_cropVisualRepresentation[position] != null)
            {
                _cropVisualRepresentation[position].GetComponent<CropRenderer>().WiltCrop();
                if (_cropVisualRepresentation[position]
                    .TryGetComponent(out PickUpInteraction interaction))
                {
                    Destroy(interaction);
                }
            }
            else
            {
                Debug.LogError($"There is no CROP at position {position}", gameObject);
            }
        }

        internal PickUpInteraction MakeCropCollectable(Vector3Int position, CropData cropData, int v, ItemDatabaseSO itemDatabase)
        {
            throw new NotImplementedException();
        }
    }
} 