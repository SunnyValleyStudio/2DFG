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
    }
}
