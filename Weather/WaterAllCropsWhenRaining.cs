using FarmGame.Farming;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Weather
{
    public class WaterAllCropsWhenRaining : MonoBehaviour
    {
        [SerializeField]
        private WeatherSystem _weatherSystem;

        private void Awake()
        {
            FieldController fieldController = FindObjectOfType<FieldController>();
            if(fieldController != null && _weatherSystem != null)
            {
                _weatherSystem.OnRaining += fieldController.WaterAllCrops;
            }
        }
    }
}
