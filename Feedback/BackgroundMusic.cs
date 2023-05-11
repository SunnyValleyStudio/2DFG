using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Feedback
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField]
        private List<AudioClip> _songToPlay = new List<AudioClip>();

        [SerializeField]
        private AudioSource _audioSource;
        private int _currentlyPlayedSongIndex = 0;

        private float _maxVolume;

        [SerializeField]
        private float _fullVolumeDelay = 0.5f;

        private void Start()
        {
            _maxVolume = _audioSource.volume;
            StartCoroutine(EaseInVolume());
            PlaySong(_songToPlay[_currentlyPlayedSongIndex]);
        }

        private void PlaySong(AudioClip song)
        {
            _audioSource.clip = song;
            _audioSource.Play();
            StartCoroutine(PlayNextSong());
        }

        private IEnumerator PlayNextSong()
        {
            yield return new WaitUntil(() => _audioSource.isPlaying == false);
            _currentlyPlayedSongIndex
                = _currentlyPlayedSongIndex + 1 >= _songToPlay.Count ?
                0 :
                _currentlyPlayedSongIndex + 1;
            StartCoroutine(EaseInVolume());
            PlaySong(_songToPlay[_currentlyPlayedSongIndex]);
        }

        private IEnumerator EaseInVolume()
        {
            float currentTime = 0.01f;
            while(currentTime < _fullVolumeDelay)
            {
                currentTime += Time.deltaTime;
                _audioSource.volume = _maxVolume*Mathf.Clamp01((currentTime/_fullVolumeDelay));
                yield return null;
            }
            _audioSource.volume = _maxVolume;
        }
    }
}
