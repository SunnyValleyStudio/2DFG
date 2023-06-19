using FarmGame.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarmGame.DataStorage
{
    [Serializable]
    public class ItemDescription
    {
        public string Name;

        [Header("General Data:"), Space]
        public int ID = -1;
        public Sprite Image;
        public string Description;
        public bool CanBeStacked = false;
        public int StackQuantity = -1;

        [Header("Item data:"), Space]
        public bool CanThrowAway = true;
        public bool Consumable = false;
        public int EnergyBoost;
        public int Price;

        [Header("Tools data:"), Space]
        public ToolType ToolType = ToolType.None;
        public Vector2Int ToolRange = Vector2Int.zero;
        public RuntimeAnimatorController ToolAnimator;

        [Header("Crop data:"), Space]
        public int CropTypeIndex = -1;

        [Header("Item Vizualization Data"), Space]
        public GameObject Prefab;

        public string GetDescription()
        {
            StringBuilder sb = new();
            sb.Append(Description);
            sb.Append("\n");
            if(ToolType == ToolType.None && CropTypeIndex == -1) 
            {
                if (Price > 0)
                    sb.Append($"Price: {Prefab} $ \n");
                if (Consumable)
                    sb.Append($"Energy Boost: {EnergyBoost} \n");
            }
            return sb.ToString();
        }
    }
}
