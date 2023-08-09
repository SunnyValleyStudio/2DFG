using FarmGame.TimeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FarmGame.UI
{
    public class PlayerCalendarUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _seaseonTxt, _dayTxt, _timeTxt;
        private TimeManager _timeManager;

        private void OnEnable()
        {
            _timeManager = FindObjectOfType<TimeManager>(true);
            Debug.Assert(_timeManager != null, "Can't find TimeManager", gameObject);
            _timeManager.OnClockProgress += UpdateAllUIElements;
        }

        private void UpdateAllUIElements(object sender, TimeEventArgs timeArgs)
        {
            UpdateTime(timeArgs.CurrentTime);
            UpdateDay(timeArgs.CurrentDay, timeArgs.WeekDay);
            UpdateSeason(timeArgs.CurrentSeason);
        }

        private void UpdateSeason(int currentSeason)
        {
            _seaseonTxt.text = CalendarNamesHelper.GetSeasonName(currentSeason);
        }

        private void UpdateDay(int currentDayIndex, int weekDay)
        {
            _dayTxt.text = $"{currentDayIndex} ({CalendarNamesHelper.GetWeekDayName(weekDay)})";
        }

        private void UpdateTime(TimeSpan currentTime)
        {
            _timeTxt.text = currentTime.ToString(@"hh\:mm");
        }
        private void OnDisable()
        {
            _timeManager.OnClockProgress -= UpdateAllUIElements;
        }

    }
}
