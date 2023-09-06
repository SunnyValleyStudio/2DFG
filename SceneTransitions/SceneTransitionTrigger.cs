using FarmGame.Agent;
using FarmGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.SceneTransitions
{
    public class SceneTransitionTrigger : MonoBehaviour
    {
        private SceneTransitionManager _sceneManager;
        [SerializeField]
        private string _sceneReferenceName;
        [field: SerializeField]
        public Transform SpawnPoint { get; set; }
        public bool Triggered { get; set; }

        [field: SerializeField]
        public int SceneTriggerID { get; set; }

        private void Awake()
        {
            _sceneManager = FindObjectOfType<SceneTransitionManager>();
            if (SpawnPoint == null)
                Debug.LogError("SpawnPoint can't be NULL", gameObject);
            if (string.IsNullOrEmpty(_sceneReferenceName))
                Debug.LogError("Scene name can't be empty", gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponentInParent<Player>().Blocked = true;
                ScreenTransitionEffect effect 
                    = FindObjectOfType<ScreenTransitionEffect>();
                effect.OnTransitionFinished += () =>
                {
                    Triggered = true;
                    _sceneManager.LoadScene(_sceneReferenceName);
                };
                effect.PlayTransition(false);
            }
        }
    }
}
