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
        private TileBase _preparedFieldTile, _wateredFieldTile;

        Dictionary<Vector3Int, GameObject> _cropVisualRepresentation = new();
        [SerializeField]
        private GameObject _cropPrefab;

        public Vector3Int GetTilemapTilePosition(Vector3 worldPosition)
            => _preparedFieldTilemap.WorldToCell(worldPosition);

        public void PrepareFieldAt(Vector3Int fieldCellPosition, bool watered = false)
        {
            TileBase tile = watered ? _wateredFieldTile : _preparedFieldTile;
            _preparedFieldTilemap.SetTile(fieldCellPosition, tile);
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

        public PickUpInteraction MakeCropCollectable(Vector3Int position, CropData cropData, 
            int quality, ItemDatabaseSO itemDatabase)
        {
            GameObject cropObject = _cropVisualRepresentation[position];

            ItemData itemData = cropObject.AddComponent<ItemData>();
            itemData.itemDatabaseIndex = cropData.ProducedItemID;
            itemData.itemCount = cropData.ProducedCount;
            itemData.itemQuality = quality;

            PickUpInteraction interaction = cropObject.AddComponent<PickUpInteraction>();
            interaction.ItemDatabase = itemDatabase;
            interaction.UsableTools = cropData.GetCollectTools;
            interaction.OnPickUp = new();

            return interaction;
        }

        public void ClearPreparedFields()
        {
            _preparedFieldTilemap.ClearAllTiles();
        }

        public void RemoveCropAt(Vector3Int position)
        {
            if(_cropVisualRepresentation.ContainsKey(position)) 
                Destroy(_cropVisualRepresentation[position]);
            _cropVisualRepresentation.Remove(position);
        }

        public void WaterCropAt(Vector3Int tilePosition)
        {
            _preparedFieldTilemap.SetTile(tilePosition, _wateredFieldTile);
        }
    }
} 