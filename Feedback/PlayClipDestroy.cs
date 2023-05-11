using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Feedback
{
    public class PlayClipDestroy : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        public void PlayClip(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
            Destroy(gameObject, clip.length);
        }
    }
}
