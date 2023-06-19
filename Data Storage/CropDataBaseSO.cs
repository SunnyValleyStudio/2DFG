using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarmGame.DataStorage
{
    [CreateAssetMenu]
    public class CropDataBaseSO : ScriptableObject
    {
        [SerializeField]
        private List<CropData> _cropData = new();
        public CropData GetDataForID(int cropTypeIndex)
        {
            return _cropData.Where(crop => crop.ID == cropTypeIndex).FirstOrDefault();
        }
    }
}