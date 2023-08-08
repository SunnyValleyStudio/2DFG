using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.TimeSystem
{
    public class TimeManager : MonoBehaviour
    {
        public event EventHandler<TimeEventArgs> OnClockProgress, OnDayProgress;

        [SerializeField]
        private TimeSpan _currentTime;
        [SerializeField]
        private float _passedTime = 0;
        [SerializeField]
        private int wakeUpHour = 6;

        private GameCalendar _calendar;

        //1 second == 10 in-game minutes
        private static int REAL_TO_GAME_TIME_CONVERSION = 1;

        private void Start()
        {
            if(_calendar == null)
            {
                _calendar = new GameCalendar();
                _calendar.OnSeaseonChanged += (seasonIndex) => SendDayUpdateEvent(true);
                _currentTime = new TimeSpan(_currentTime.Days, wakeUpHour, 0, 0);
            }
            SendTimeUpdateEvent();
        }

        private void SendTimeUpdateEvent()
        {
            OnClockProgress?.Invoke(this, new TimeEventArgs(false, _calendar,
                _currentTime, true));
        }

        private void SendDayUpdateEvent(bool seasonChanged, bool theSameDay = false)
        {
            OnDayProgress?.Invoke(this, new TimeEventArgs(seasonChanged, _calendar, 
                _currentTime, theSameDay));
        }

        public void GoToNextDay()
        {
            bool theSameDay = false;
            int oldSeasonIndex = _calendar.Season;
            int tempWakeUpHour = wakeUpHour;
            if(_currentTime.Hours >= 0 && _currentTime.Hours < 6)
            {
                tempWakeUpHour += _currentTime.Hours;
                theSameDay = true;
            }
            else
            {
                _calendar.ProgressTime();
            }

            _currentTime = new TimeSpan(_calendar.Day, wakeUpHour, 0, 0);
            _passedTime = 0;
            SendDayUpdateEvent(oldSeasonIndex != _calendar.Season, theSameDay);
            SendTimeUpdateEvent();
            Debug.Log(ToString());
        }

        public override string ToString()
        {
            return $"Season: {CalendarNamesHelper.GetSeasonName(_calendar.Season)}," +
                $"Day: {_calendar.Day}, " +
                $"Week day name: {CalendarNamesHelper.GetWeekDayName(_calendar.WeekDay)}";
        }

        private void Update()
        {
            if(_passedTime < REAL_TO_GAME_TIME_CONVERSION)
            {
                _passedTime += Time.deltaTime;
            }
            else
            {
                _passedTime = _passedTime - REAL_TO_GAME_TIME_CONVERSION;
                _currentTime = _currentTime.Add(new TimeSpan(0, 10, 0));
                SendTimeUpdateEvent();
                if(_currentTime.Hours == 0 && _currentTime.Minutes == 0)
                {
                    int oldSeasonIndex = _calendar.Season;
                    _calendar.ProgressTime();
                    SendDayUpdateEvent(oldSeasonIndex != _calendar.Season);
                }
            }
        }
    }

    public class TimeEventArgs : EventArgs
    {
        public bool SeasonChanged { get; private set; }
        public int CurrentDay { get; private set; }
        public int WeekDay { get; private set; }
        public int CurrentSeason { get; private set; }
        public TimeSpan CurrentTime { get; private set; }
        public bool TheSameDay { get; private set; }

        public TimeEventArgs(bool seasonChanged, int currentDay, int weekDay,
    int currentSeason, TimeSpan currentTime, bool theSameDay)
        {
            SeasonChanged = seasonChanged;
            CurrentDay = currentDay;
            WeekDay = weekDay;
            CurrentSeason = currentSeason;
            CurrentTime = currentTime;
            TheSameDay = theSameDay;
        }

        public TimeEventArgs(bool seasonChanged, GameCalendar calendar, 
            TimeSpan currentTime, bool theSameDay)
        {
            SeasonChanged = seasonChanged;
            CurrentDay = calendar.Day;
            WeekDay = calendar.WeekDay;
            CurrentSeason = calendar.Season;
            CurrentTime = currentTime;
            TheSameDay = theSameDay;
        }
    }
}
