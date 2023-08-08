using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.TimeSystem
{
    public static class CalendarNamesHelper 
    {
        public static string GetWeekDayName(int weekDayIndex)
            => weekDayIndex switch
            {
                0 => "Mon",
                1 => "Tue",
                2 => "Wed",
                3 => "Thu",
                4 => "Fri",
                5 => "Sat",
                6 => "Sun",
                _ => throw new Exception($"Unhandeled week day index {weekDayIndex}")
            };

        public static string GetSeasonName(int seasonIndex)
            => seasonIndex switch
            {
                0 => "Spring",
                1 => "Summer",
                2 => "Autumn",
                3 => "Winter",
                _ => throw new Exception($"Unhandeled season idex {seasonIndex}")
            };
    }
}
