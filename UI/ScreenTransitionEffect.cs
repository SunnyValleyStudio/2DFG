using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.UI
{
    public class ScreenTransitionEffect : MonoBehaviour
    {
        [SerializeField]
        private float _duration = 0.5f;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        float _timePassed = 0;

        public event Action OnTransitionFinished;

        public void PlayTransition(bool fadeIn)
        {
            _timePassed = 0;
            if (fadeIn)
            {
                _canvasGroup.alpha = 1;
            }
            else
            {
                _canvasGroup.alpha = 0;
            }
            StartCoroutine(Transition(fadeIn));
        }

        private IEnumerator Transition(bool fadeIn)
        {
            while(_timePassed < _duration)
            {
                _timePassed += Time.deltaTime;
                float alphaModifier = Mathf.Clamp01(_timePassed / _duration);
                _canvasGroup.alpha = fadeIn ? 1 - alphaModifier : alphaModifier;
                yield return null;
            }
            _canvasGroup.alpha = fadeIn ? 0 : 1;
            OnTransitionFinished?.Invoke();
        }
    }
}
