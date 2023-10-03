using FarmGame.TimeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FarmGame.DayNight
{
    public class DayNightSystem : MonoBehaviour
    {
        [SerializeField]
        private Light2D _globalLight;

        TimeManager _timeManager;

        [SerializeField]
        private int _sunRiseHour = 5, _sunUpHour = 6, 
            _sunSetHour = 19, _sunDownHour = 20;

        [SerializeField]
        private float _sunMaxIntensity = 1, _sunMinIntensity = 0.05f;

        [SerializeField]
        private TimeOfDay _currentTimeOfDay = TimeOfDay.Day;

        public event Action<bool> OnNightTime;

        private void Awake()
        {
            _timeManager = FindObjectOfType<TimeManager>(true);
            if(_timeManager != null)
            {
                _timeManager.OnClockProgress += AffectLight;
            }

            foreach (LightSource light in FindObjectsOfType<LightSource>())
            {
                OnNightTime += light.ToggleLight;
            }
        }

        private void AffectLight(object sender, TimeEventArgs e)
        {
            int currentHour = e.CurrentTime.Hours;
            TimeOfDay tempTimeOfDay = TimeOfDay.Day;
            float valueToSet = 1;

            if(currentHour >= _sunRiseHour &&  currentHour < _sunUpHour)
            {
                tempTimeOfDay = TimeOfDay.SunRise;
                valueToSet = Mathf.Clamp(e.CurrentTime.Minutes / 60f
                    , _sunMinIntensity, _sunMaxIntensity);
            }else if(currentHour >= _sunUpHour && currentHour < _sunSetHour)
            {
                tempTimeOfDay = TimeOfDay.Day;
                valueToSet = _sunMaxIntensity;
            }else if(currentHour >= _sunSetHour && currentHour < _sunDownHour)
            {
                tempTimeOfDay = TimeOfDay.SunSet;
                valueToSet = Mathf.Clamp(1 - e.CurrentTime.Minutes / 60f
                    , _sunMinIntensity, _sunMaxIntensity);
            }
            else
            {
                tempTimeOfDay = TimeOfDay.Night;
                valueToSet = _sunMinIntensity;
            }

            if(tempTimeOfDay != _currentTimeOfDay || tempTimeOfDay == TimeOfDay.SunSet ||
                tempTimeOfDay == TimeOfDay.SunRise)
            {
                if(tempTimeOfDay == TimeOfDay.Day)
                {
                    OnNightTime?.Invoke(false);
                }else if(tempTimeOfDay == TimeOfDay.Night)
                {
                    OnNightTime?.Invoke(true);
                }

                _currentTimeOfDay = tempTimeOfDay;
                Debug.Log($"Setting light intensity to {valueToSet} because it is {_currentTimeOfDay}");
                _globalLight.intensity = valueToSet;
            }
        }
    }

    public enum TimeOfDay
    {
        Day,
        Night,
        SunSet,
        SunRise
    }
}
