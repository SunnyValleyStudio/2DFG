using FarmGame.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.SceneTransitions
{
    public class SceneTransitionManager : MonoBehaviour, ISavable
    {
        [SerializeField]
        private bool _isMainMenu = false;
        public string LoadedSceneName { get; private set; } = string.Empty;

        public int SaveID => SaveIDRepositor.SCENE_TRANSITION_MANAGER;

        public event Action OnBeforeLoadScene;

        public string GetData()
        {
            if (_isMainMenu)
                return LoadedSceneName;
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        public void LoadScene(string sceneReferenceName)
        {
            OnBeforeLoadScene?.Invoke();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneReferenceName);
        }

        public void RestoreData(string data)
        {
            LoadedSceneName = data;
        }
    }
}
