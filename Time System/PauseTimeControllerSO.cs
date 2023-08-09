using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.TimeSystem
{
    [CreateAssetMenu]
    public class PauseTimeControllerSO : ScriptableObject
    {
        public void SetTimePause(bool timeFreez)
        {
            if(timeFreez)
            {
                Debug.Log($"<b><size=15>Time</size></b> paused <color=red> {timeFreez} </color>");
            }
            else
            {
                Debug.Log($"<b><size=15>Time</size></b> paused <color=green> {timeFreez} </color>");
            }
            Time.timeScale = timeFreez ? 0 : 1;
        }
    }
}
