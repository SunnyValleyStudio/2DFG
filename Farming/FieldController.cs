using FarmGame.DataStorage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Farming
{
    public class FieldController : MonoBehaviour
    {
        private FieldRenderer _fieldRenderer;
        [SerializeField]
        private FieldData _fieldData;
        [SerializeField]
        private CropDataBaseSO _cropDatabase;

        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _preparedFieldSound, _placeSeedSound;

        private void Awake()
        {
            _fieldRenderer = FindObjectOfType<FieldRenderer>(true);
            if(_fieldData == null)
            {
                _fieldData = FindObjectOfType<FieldData>();
                if (_fieldData == null)
                    Debug.LogError("Can't find Field Data", gameObject);
            }
        }

        public void PrepareFieldAt(Vector2 worldPosition)
        {
            if(_fieldRenderer == null) return;
            Vector3Int tilePositon = _fieldRenderer.GetTilemapTilePosition(worldPosition);
            if(_fieldData.crops.ContainsKey(tilePositon) )
            {
                return;
            }
            _fieldRenderer.PrepareFieldAt(tilePositon);
            _fieldData.preparedFields.Add(tilePositon);
            _audioSource.PlayOneShot(_preparedFieldSound);
        }

        public bool CanIPlaceCropsHere(Vector2 position)
        {
            Vector3Int tilePosition = _fieldRenderer.GetTilemapTilePosition(position);
            return _fieldData.preparedFields.Contains(tilePosition) &&
                _fieldData.crops.ContainsKey(tilePosition) == false;
        }

        public void PlaceCropAt(Vector2 position, int cropID, int growthLevel = 0
            , bool playSound = true)
        {
            if (_fieldRenderer == null)
                return;
            Vector3Int tilePosition = _fieldRenderer.GetTilemapTilePosition(position);
            if(_fieldData.crops.ContainsKey(tilePosition) == false)
            {
                _fieldData.crops[tilePosition] = new Crop(cropID);
            }
            CropData data = _cropDatabase.GetDataForID(cropID);
            if(data == null) 
            {
                Debug.LogError($"No data found for id {cropID} ");
                return;
            }
            Debug.Log("Creating visualization for the crop");
            if(playSound )
            {
                _audioSource.PlayOneShot(_placeSeedSound);
            }
            PrintCropsStatus();

        }

        public void PrintCropsStatus()
        {
            _fieldData.PrintCropStatus();
        }
    }
}
