using FarmGame.TimeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Weather
{
    public class WeatherSystem : MonoBehaviour
    {
        private RainController[] _rainControllers;
        private bool _isRaining;
        [SerializeField]
        private WeatherDataSO _weatherData;
        [SerializeField]
        private bool _inHouseScene;
        private TimeManager _timeManager;
        public event Action OnRaining;

        private void Awake()
        {
            _rainControllers = FindObjectsOfType<RainController>();
            _timeManager = FindObjectOfType<TimeManager>();
            if(_timeManager != null)
            {
                _timeManager.OnClockProgress += SetUpWeatherFirst;
                _timeManager.OnClockProgress += CallOnRainEvent;
                _timeManager.OnDayProgress += SetUpWeather;
            }
        }

        private void SetUpWeather(object sender, TimeEventArgs e)
        {
            _isRaining = false;
            if(_weatherData.rainDaysInSeason.Count > e.CurrentSeason)
            {
                _isRaining = _weatherData.rainDaysInSeason[e.CurrentSeason]
                    .rainDays.Contains(e.CurrentDay);
            }

            if (_inHouseScene == false)
                ToggleRain(_isRaining);
        }

        private void ToggleRain(bool isRaining)
        {
            foreach(RainController controller in _rainControllers)
            {
                controller.ToggleRain(isRaining);
            }
        }

        private void CallOnRainEvent(object sender, TimeEventArgs e)
        {
            if(_isRaining)
            {
                OnRaining?.Invoke();
            }
        }

        private void SetUpWeatherFirst(object sender, TimeEventArgs e)
        {
            SetUpWeather(sender, e);
            _timeManager.OnClockProgress -= SetUpWeatherFirst;
        }
    }
}
