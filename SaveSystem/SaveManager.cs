using FarmGame.SceneTransitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace FarmGame.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField]
        private string _saveDataKey = "SavedDataFarm";
        private string _gameSaveFileName = "FarmGameSavedData";
        [SerializeField]
        private bool _mainMenuFlag = false;
        private List<ObjectsSaveData> _unusedData = new();

        private void Start()
        {
            if (_mainMenuFlag)
            {
                LoadDataFromFile();
                return;
            }
            LoadGameState();
            FindObjectOfType<SceneTransitionManager>().OnBeforeLoadScene 
                += SaveGameState;
        }

        private void SaveGameState()
        {
            string data = GetDataToSave();
            PlayerPrefs.SetString(_saveDataKey, data);
        }

        private string GetDataToSave()
        {
            IEnumerable<ISavable> savableObjects 
                = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();
            List<ObjectsSaveData> saveData = new();
            foreach (ISavable obj in savableObjects)
            {
                saveData.Add(new ObjectsSaveData 
                { ID = obj.SaveID, Data = obj.GetData() });
            }
            saveData.AddRange(_unusedData);
            string data = JsonUtility.ToJson(new SaveData { DataList = saveData});
            return data;
        }

        private void LoadGameState()
        {
            string dataAsString = PlayerPrefs.GetString(_saveDataKey);
            RestoreData(dataAsString);
        }

        private void RestoreData(string dataAsString)
        {
            IEnumerable<ISavable> savableObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<ISavable>();
            SaveData saveData = string.IsNullOrEmpty(dataAsString) 
                ? new SaveData() 
                : JsonUtility.FromJson<SaveData>(dataAsString);

            _unusedData.Clear();

            if(saveData.DataList != null)
            {
                _unusedData = saveData.DataList
                    .Where(data => savableObjects
                        .Any(savable => savable.SaveID == data.ID) == false)
                    .ToList();
            }

            foreach (var savable in savableObjects)
            {
                string dataToLoad = null;
                if(saveData.DataList != null && saveData.DataList.Count > 0)
                {
                    dataToLoad = saveData.DataList.FirstOrDefault
                        (dataPoint => dataPoint.ID == savable.SaveID).Data;
                }
                savable.RestoreData(dataToLoad);
            }
        }

        private void LoadDataFromFile()
        {
            string data;
            ReadFromFile(_gameSaveFileName, out data);
            RestoreData(data);
        }

        private void ReadFromFile(string gameSaveFileName, out string data)
        {
            throw new NotImplementedException();
        }

        [Serializable]
        public struct ObjectsSaveData
        {
            public int ID;
            public string Data;
        }

        [Serializable]
        public struct SaveData
        {
            public List<ObjectsSaveData> DataList;
        }
    }
}
