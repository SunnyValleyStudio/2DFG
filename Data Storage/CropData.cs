using FarmGame.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FarmGame.DataStorage
{
    [Serializable]
    public class CropData : ISerializationCallbackReceiver
    {
        public string Name;
        [Min(0)]
        public int ID;
        [Min(0)]
        public int ProducedItemID;
        public List<Sprite> Sprites;
        [Min(1)]
        public int GrowthDelayPerStage;
        [Min(1)]
        public int WiltThreshold;
        [SerializeField]
        private Seasons _growthSeason;
        [field: SerializeField]
        public int GrowthSeasonIndex { get; private set; }
        [field:SerializeField]
        public int ProducedCount { get; private set; }
        [SerializeField]
        private List<ToolType> _collectTools;
        public List<ToolType> GetCollectTools => new List<ToolType>(_collectTools);

        public void OnBeforeSerialize()
        {
            return;
        }

        public void OnAfterDeserialize()
        {
            GrowthSeasonIndex = (int)_growthSeason;
        }
    }
}

namespace FarmGame
{
    [Flags]
    public enum Seasons
    {
        Spring = 1, //0001
        Summer = 2, //0010 
        Autumn = 4, //0100
        Winter = 8  //1000
    }
}
