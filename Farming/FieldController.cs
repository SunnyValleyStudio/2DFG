using FarmGame.DataStorage;
using FarmGame.Interactions;
using FarmGame.TimeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        private ItemDatabaseSO _itemDatabase;

        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _preparedFieldSound, _placeSeedSound;

        private TimeManager _timeManager;

        private TimeEventArgs _previousTimeData;

        private void Awake()
        {
            _fieldRenderer = FindObjectOfType<FieldRenderer>(true);
            if(_fieldData == null)
            {
                _fieldData = FindObjectOfType<FieldData>();
                if (_fieldData == null)
                    Debug.LogError("Can't find Field Data", gameObject);
            }

            if (_timeManager = FindObjectOfType<TimeManager>(true))
            {
                _timeManager.OnDayProgress += AffectCrops;
            }
            else
            {
                Debug.LogWarning("Can't find TimeManager", gameObject);
            }
            
        }

        private void AffectCrops(object sender, TimeEventArgs timeArgs)
        {
            if(_previousTimeData != null && _previousTimeData.CurrentDay == timeArgs.CurrentDay)
            {
                return;
            }
            _previousTimeData = timeArgs;

            foreach (var keyValue in _fieldData.crops)
            {
                Crop crop = keyValue.Value;
                CropData data = _cropDatabase.GetDataForID(crop.ID);
                if(data == null) 
                {
                    throw new Exception($"No data for the crop with ID {crop.ID}");
                }
                if (crop.Dead)
                {
                    continue;
                }
                if (((timeArgs.CurrentSeason + 1) & data.GrowthSeasonIndex)
                    !=(timeArgs.CurrentSeason +1))
                {
                    if (timeArgs.SeasonChanged)
                        crop.Dead = true;
                    else
                        continue;
                }
                ModifyCropStatus(crop, data, keyValue.Key);
                if(crop.Regress >= data.WiltThreshold || crop.Dead)
                {
                    crop.Dead = true;
                    WiltCrop(keyValue.Key);
                }
            }
            PrintCropsStatus();
        }

        private void WiltCrop(Vector3Int position)
        {
            if(_fieldRenderer == null)
            {
                return;
            }
            Vector3Int cropPosition = _fieldRenderer.GetTilemapTilePosition(position);
            _fieldRenderer.WiltCropVisualization(cropPosition);
        }

        private void ModifyCropStatus(Crop crop, CropData cropData, Vector3Int position)
        {
            if (crop.Ready)
            {
                crop.Regress++;
            }
            else
            {
                if (crop.Watered)
                {
                    crop.Watered = false;
                    if(crop.Regress > 0)
                    {
                        crop.Regress--;
                    }
                    else
                    {
                        crop.Progress++;
                        if(crop.Progress > cropData.GrowthDelayPerStage)
                        {
                            crop.GrowthLevel++;
                            crop.Progress = 0;
                            //is the crop ready for harvest
                            if(crop.GrowthLevel == cropData.Sprites.Count - 1)
                            {
                                crop.Ready = true;
                                ClearFieldAt(position);
                                if(_fieldRenderer != null)
                                {
                                    PickUpInteraction pickUpInteraction = _fieldRenderer
                                        .MakeCropCollectable(position, cropData, crop.GetQuality(), _itemDatabase);
                                    pickUpInteraction.OnPickUp.AddListener(() =>
                                    {
                                        RemoveCropAt(position);
                                    });
                                }
                                return;
                            }
                            else
                            {
                                UpdateCropAt(position, crop.ID, crop.GrowthLevel);
                            }
                        }
                    }
                }
                else
                {
                    if(crop.GrowthLevel > 0)
                    {
                        crop.Regress++;
                    }
                }
            }
        }

        private void UpdateCropAt(Vector3Int position, int iD, int growthLevel)
        {
            throw new NotImplementedException();
        }

        private void RemoveCropAt(Vector3Int position)
        {
            throw new NotImplementedException();
        }

        private void ClearFieldAt(Vector3Int position)
        {
            throw new NotImplementedException();
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
            _fieldRenderer.CreateCropVisualization(tilePosition, data.Sprites[growthLevel],
                growthLevel > 0);
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
