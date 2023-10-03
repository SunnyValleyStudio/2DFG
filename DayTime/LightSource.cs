using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FarmGame.DayNight
{
    public class LightSource : MonoBehaviour
    {
        [SerializeField]
        private Light2D _light;
        internal void ToggleLight(bool value)
        {
            _light.enabled = value;
        }

    }
}
