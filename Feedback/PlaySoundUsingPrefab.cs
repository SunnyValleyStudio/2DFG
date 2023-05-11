using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Feedback
{
    public class PlaySoundUsingPrefab : MonoBehaviour
    {
        [SerializeField]
        private PlayClipDestroy _audioPrefab;

        [SerializeField]
        private AudioClip[] _audioClips;

        private void Awake()
        {
            Debug.Assert(_audioClips != null && _audioClips.Length > 0, 
                "AudioClips array must have some audio clips", 
                gameObject);
        }

        public void CreateSoundObject()
        {
            PlayClipDestroy audioObject 
                = Instantiate(_audioPrefab, transform.position, Quaternion.identity);
            audioObject.PlayClip(_audioClips[Random.Range(0,_audioClips.Length)]);
        }
    }
}
