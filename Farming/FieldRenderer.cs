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

        public Vector3Int GetTilemapTilePosition(Vector3 worldPosition)
            => _preparedFieldTilemap.WorldToCell(worldPosition);

        public void PrepareFieldAt(Vector3Int fieldCellPosition)
        {
            _preparedFieldTilemap.SetTile(fieldCellPosition,_preparedFieldTile);
        }
    }
}
