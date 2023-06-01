using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Farming
{
    public class FieldController : MonoBehaviour
    {
        private FieldRenderer _fieldRenderer;
        [SerializeField]
        private List<Vector3Int> _preparedFields = new();

        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _preparedFieldSound;

        private void Awake()
        {
            _fieldRenderer = FindObjectOfType<FieldRenderer>(true);
        }

        public void PrepareFieldAt(Vector2 worldPosition)
        {
            if(_fieldRenderer == null) return;
            Vector3Int tilePositon = _fieldRenderer.GetTilemapTilePosition(worldPosition);
            if(_preparedFields.Contains(tilePositon) )
            {
                return;
            }
            _fieldRenderer.PrepareFieldAt(tilePositon);
            _preparedFields.Add(tilePositon);
            _audioSource.PlayOneShot(_preparedFieldSound);
        }
    }
}
