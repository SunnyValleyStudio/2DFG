using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Weather
{
    [CreateAssetMenu]
    public class WeatherDataSO : ScriptableObject
    {
        public List<RainDays> rainDaysInSeason;

    }

    [Serializable]
    public class RainDays
    {
        public List<int> rainDays;
    }
}
