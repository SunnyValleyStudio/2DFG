using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarmGame.DataStorage
{
    [CreateAssetMenu]
    public class ItemDatabaseSO : ScriptableObject
    {
        [SerializeField]
        private CropDataBaseSO _cropDatabase;
        [SerializeField]
        private List<ItemDescription> _gameItems = new();

        private void Awake()
        {
            for (int i = 0; i < _gameItems.Count; i++)
            {
                if (_gameItems[i] != null && _gameItems[i].ID == -1)
                    _gameItems[i].ID = i;
            }
        }

        public ItemDescription GetItemData(int ID)
            => _gameItems.Where(item => item.ID == ID).FirstOrDefault();

        public string GetItemDescription(int ID)
        {
            ItemDescription item = GetItemData(ID);
            if(item == null)
                return null;
            string baseDescription = item.GetDescription();
            if(item.CropTypeIndex > -1)
            {
                CropData data = _cropDatabase.GetDataForID(item.CropTypeIndex);
                if(data != null)
                {
                    baseDescription += $"Growth Season: {data.GrowthSeasonIndex} \n";
                }
            }
            return baseDescription; 
        }
    }
}
