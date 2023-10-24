using FarmGame.DataStorage;
using FarmGame.Interactions;
using FarmGame.SaveSystem;
using FarmGame.TimeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine;

namespace FarmGame.Farming
{
    public class FieldController : MonoBehaviour, ISavable
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
        private AudioClip _preparedFieldSound, _placeSeedSound, _wateringFieldSound;

        private TimeManager _timeManager;

        private TimeEventArgs _previousTimeData;

        public int SaveID => SaveIDRepositor.FIELD_CONTROLLER_ID;

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

        private void Start()
        {
            if(_fieldRenderer != null)
            {
                RecreatePreparedFieldPositions();
                RecreateFields();
            }
        }

        private void RecreateFields()
        {
            if (_fieldRenderer == null)
                return;
            _fieldRenderer.ClearCropVisualization();
            foreach (var item in _fieldData.crops)
            {
                CropData data = _cropDatabase.GetDataForID(item.Value.ID);
                if(data == null)
                {
                    Debug.LogError($"No data for crop with ID {item.Value.ID}", gameObject);
                    return;
                }
                PlaceCropAt(new Vector2(item.Key.x, item.Key.y), item.Value.ID,
                    item.Value.GrowthLevel, false);
                if (item.Value.Dead)
                {
                    WiltCrop(item.Key);
                    return;
                }
                if (item.Value.Ready)
                {
                    PickUpInteraction interaction = _fieldRenderer.MakeCropCollectable(
                        item.Key, data,
                        item.Value.GetQuality(), _itemDatabase);
                    interaction.OnPickUp.AddListener(() =>
                    {
                        RemoveCropAt(item.Key);
                    });
                }
            }
        }

        public void AddDebrisAt(Vector3 position, GameObject debrisRepresentation)
        {
            Vector3Int debrisTilePosition 
                = _fieldRenderer.GetTilemapTilePosition(position);
            if (_fieldData.removedDebris.Contains(debrisTilePosition))
            {
                Debug.Log($"Destroing Debris {debrisRepresentation.name} " +
                    $"at {debrisTilePosition}");
                Destroy(debrisRepresentation);
            }
            else
            {
                _fieldData.debris.Add(debrisTilePosition);
                _fieldRenderer
                    .AddDebrisVisualization(debrisTilePosition, debrisRepresentation);
            }

        }

        public void RemoveDebris(Vector3 position)
        {
            if (_fieldRenderer == null)
                return;
            Vector3Int debrisTilePosition 
                = _fieldRenderer.GetTilemapTilePosition(position);
            _fieldData.removedDebris.Add(debrisTilePosition);
            _fieldData.debris.Remove(debrisTilePosition);
            _fieldRenderer.RemoveDebriVisualization(debrisTilePosition);
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
                //crop.Watered = true;
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
                    if (_fieldRenderer != null)
                        _fieldRenderer.PrepareFieldAt(position);
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
            if (_fieldRenderer == null)
                return;
            Vector3Int tilePosition = _fieldRenderer.GetTilemapTilePosition(position);
            CropData data = _cropDatabase.GetDataForID(iD);
            if(data == null)
            {
                Debug.LogError($"No data for crop with id {iD}", gameObject);
                return;
            }
            else
            {
                _fieldRenderer.UpdateCropVisualization(tilePosition, data.Sprites[growthLevel]
                    , growthLevel > 0);
                if (growthLevel < 1)
                    _audioSource.PlayOneShot(_placeSeedSound);
            }
        }

        private void RemoveCropAt(Vector3Int position)
        {
            _fieldData.crops.Remove(position);
            if(_fieldRenderer != null)
            {
                _fieldRenderer.RemoveCropAt(position);
            }
        }

        private void ClearFieldAt(Vector3Int position)
        {
            _fieldData.preparedFields.Remove(position);
            RecreatePreparedFieldPositions();
        }

        private void RecreatePreparedFieldPositions()
        {
            if (_fieldRenderer == null)
                return;
            _fieldRenderer.ClearPreparedFields();
            foreach (var fieldPosition in _fieldData.preparedFields)
            {
                bool watered = _fieldData.crops.ContainsKey(fieldPosition) ?
                    _fieldData.crops[fieldPosition].Watered : false;
                _fieldRenderer.PrepareFieldAt(fieldPosition, watered);
            }
        }

        public void PrepareFieldAt(Vector2 worldPosition)
        {
            if(_fieldRenderer == null) return;
            Vector3Int tilePositon = _fieldRenderer.GetTilemapTilePosition(worldPosition);
            if(_fieldData.preparedFields.Contains(tilePositon))
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

        public bool IsThereCropAt(Vector2 pos)
        => _fieldData.crops.ContainsKey(_fieldRenderer.GetTilemapTilePosition(pos));

        public void WaterCropAt(Vector2 pos)
        {
            Vector3Int tilePosition = _fieldRenderer.GetTilemapTilePosition(pos);
            bool result = WaterCropUpdateData(tilePosition);
            if (result == false)
                return;
            _fieldRenderer.WaterCropAt(tilePosition);
            _audioSource.PlayOneShot(_wateringFieldSound);
        }

        private bool WaterCropUpdateData(Vector3Int tilePosition)
        {
            if(_fieldData.crops.ContainsKey(tilePosition) == false)
                return false;
            _fieldData.crops[tilePosition].Watered = true;
            return true;
        }

        public string GetData()
        {
            FieldControllerSaveData data = new()
            {
                preparedFields = _fieldData.preparedFields,
                cropFields = new List<Vector3Int>(_fieldData.crops.Keys),
                cropData = _fieldData.crops.Values.Select(crop => crop.GetSaveData()).ToList(),
                removedDebris = new List<Vector3Int>(_fieldData.removedDebris),
                previousCurrentDay = _previousTimeData == null ? -1 
                : _previousTimeData.CurrentDay
            };
            return JsonUtility.ToJson(data);
        }

        public void RestoreData(string data)
        {
            if (string.IsNullOrEmpty(data))
                return;
            FieldControllerSaveData loadedData 
                = JsonUtility.FromJson<FieldControllerSaveData>(data);
            _fieldData.preparedFields = loadedData.preparedFields;
            _fieldData.removedDebris = new HashSet<Vector3Int>(loadedData.removedDebris);
            for (int i = 0; i < loadedData.cropFields.Count; i++)
            {
                Crop c = Crop.RestoreData(loadedData.cropData[i]);
                _fieldData.crops.Add(loadedData.cropFields[i], c);
            }
            if (loadedData.previousCurrentDay == -1)
                _previousTimeData = null;
            else
                _previousTimeData 
                    = new TimeEventArgs(false, 
                    loadedData.previousCurrentDay, 
                    1, 1, new TimeSpan(), false);
            PrintCropsStatus();
        }

        internal void WaterAllCrops()
        {
            foreach (var cropPosition in _fieldData.crops.Keys)
            {
                if (_fieldData.crops[cropPosition].Watered)
                    continue;
                WaterCropUpdateData(cropPosition);
                if(_fieldRenderer != null)
                    _fieldRenderer.WaterCropAt(cropPosition, false);
            }
        }

        [Serializable]
        public struct FieldControllerSaveData
        {
            public List<Vector3Int> preparedFields;
            public List<Vector3Int> cropFields;
            public List<string> cropData;
            public List<Vector3Int> removedDebris;
            public int previousCurrentDay;
        }
    }
}
