using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Feedback
{
    public class PlaySoundSimple : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _soundToPlay;
        [SerializeField]
        private AudioSource _audioSource;

        public void StartPlaying()
        {
            _audioSource.PlayOneShot(_soundToPlay);
        }
    }
}
