using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FarmGame
{
    public class FieldPositionValidator : MonoBehaviour
    {
        [SerializeField]
        private Tilemap _fieldTilemap;
        [SerializeField]
        private string _fieldTilemapTag = "Field";

        private void Awake()
        {
            if(_fieldTilemap == null) 
            {
                _fieldTilemap = FindObjectsOfType<Tilemap>()
                    .FirstOrDefault( tilemap => tilemap.CompareTag(_fieldTilemapTag));
            }
            Debug.Assert(_fieldTilemap != null,"FieldTilemap reference must be assigned",gameObject);
        }

        public bool IsItFieldTile(Vector2 worldPosition)
        {
            return _fieldTilemap.HasTile(_fieldTilemap.WorldToCell(worldPosition));
        }

        public List<Vector2> GetValidFieldTiles(List<Vector2> worldPositions)
        {
            List<Vector2> validPositions = new();
            foreach(Vector2 position in worldPositions)
            {
                Vector3Int tilemapPositon = _fieldTilemap.WorldToCell(position);
                if (_fieldTilemap.HasTile(tilemapPositon))
                {
                    validPositions.Add(_fieldTilemap.GetCellCenterWorld(tilemapPositon));
                }
            }
            return validPositions;
        }

        public Vector2 GetValidFieldTile(Vector2 worldPosition)
        {
            if (IsItFieldTile(worldPosition) == false)
                throw new System.Exception("POsition is invalid for our field Tilemap");
            return _fieldTilemap.GetCellCenterWorld(_fieldTilemap.WorldToCell(worldPosition));
        }
    }
}
