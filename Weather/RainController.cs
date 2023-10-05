using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Weather
{
    public class RainController : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _rainSystem;
        private void Awake()
        {
            _rainSystem = GetComponent<ParticleSystem>();
        }

        internal void ToggleRain(bool isRaining)
        {
            if(_rainSystem != null)
            {
                if(isRaining)
                {
                    _rainSystem.Play();
                }
                else
                {
                    _rainSystem.Stop();
                }
            }
        }


    }
}
