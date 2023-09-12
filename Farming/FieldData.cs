using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Farming
{
    public class FieldData : MonoBehaviour
    {
        public Dictionary<Vector3Int, Crop> crops = new();

        public List<Vector3Int> preparedFields = new();
        public HashSet<Vector3Int> debris = new();
        public HashSet<Vector3Int> removedDebris = new();

        public void PrintCropStatus()
        {
            foreach (var keyValue in crops)
            {
                Debug.Log($"Crop at {keyValue.Key} is: {keyValue.Value}");
            }
        }
    }

    public class Crop
    {
        public bool Watered { get; set; }
        public int Progress { get; set; }
        public int Regress { get; set; }
        public int ID { get; set; }
        public bool Ready { get; set; }
        public bool Dead { get; set; }

        public int GrowthLevel { get; set; } = 0;

        public Crop(int ID)
        {
            this.ID = ID;
        }
         
        public override string ToString()
        {
            return $"id {this.ID}, Ready {this.Ready}, Dead {this.Dead}, Level {this.GrowthLevel}" +
                $",Regress {this.Regress}, Progree {this.Progress}, Watered {this.Watered}";
        }

        public int GetQuality()
        {
            return 1;
        }

        public string GetSaveData()
        {
            CropSaveData data = new()
            {
                Watered = Watered,
                Progress = Progress,
                Regress = Regress,
                ID = ID,
                Ready = Ready,
                Dead = Dead,
                GrowthLevel = GrowthLevel
            };
            return JsonUtility.ToJson(data);
        }

        public static Crop RestoreData(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            CropSaveData loadedData = JsonUtility.FromJson<CropSaveData>(data);
            Crop crop = new(loadedData.ID)
            {
                Watered = loadedData.Watered,
                Progress = loadedData.Progress,
                Regress = loadedData.Regress,
                Ready = loadedData.Ready,
                Dead = loadedData.Dead,
                GrowthLevel = loadedData.GrowthLevel
            };
            return crop;
        }

        [Serializable]
        public struct CropSaveData
        {
            public bool Watered;
            public int Progress;
            public int Regress;
            public int ID;
            public bool Ready;
            public bool Dead;
            public int GrowthLevel;
        }
    }
}
