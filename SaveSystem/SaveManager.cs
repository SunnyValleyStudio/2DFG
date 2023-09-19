using FarmGame.SceneTransitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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

        public bool SaveDataPresent { get; private set; }

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

        public void SaveDataToFile()
        {
            string data = GetDataToSave();
            if(WriteToFile(_gameSaveFileName, data))
            {
                Debug.Log("Data Saved to a file");
            }
        }

        private bool WriteToFile(string gameSaveFileName, string data)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, 
                gameSaveFileName +".txt");
            Debug.Log($"{fullPath}");

            try
            {
                File.WriteAllText(fullPath, data);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error while savaing data to a file " + e.Message);
            }
            return false;
        }

        public void SaveGameState()
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

        private bool ReadFromFile(string gameSaveFileName, out string data)
        {
            string fullPath = Path.Combine(Application.persistentDataPath,
                gameSaveFileName + ".txt");
            data = string.Empty;
            try
            {
                data = File.ReadAllText(fullPath);
                SaveDataPresent = string.IsNullOrEmpty(data) == false;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error when loadin the file " + e.Message);
                
            }
            return false;
        }

        public void ResetSavedData()
        {
            PlayerPrefs.DeleteKey(_saveDataKey);
            WriteToFile(_gameSaveFileName, string.Empty);
            _unusedData.Clear();
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
