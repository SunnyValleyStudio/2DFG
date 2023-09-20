using FarmGame.SaveSystem;
using FarmGame.SceneTransitions;
using FarmGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private string _inHouseSceneName;

        [SerializeField]
        private ScreenTransitionEffect _screenTransitionEffect;
        [SerializeField]
        private SceneTransitionManager _sceneTransitionManager;
        [SerializeField]
        private SaveManager _saveManager;

        public UnityEvent OnNoSaveData;

        private void Start()
        {
            if (_saveManager.SaveDataPresent == false)
                OnNoSaveData?.Invoke();
        }

        public void CreateNewGame()
        {
            _screenTransitionEffect.PlayTransition(false);
            _saveManager.ResetSavedData();
            StartCoroutine(LoadScene(_inHouseSceneName));
        }

        public void LoadSavedData()
        {
            _screenTransitionEffect.PlayTransition(false);
            _saveManager.SaveGameState();
            StartCoroutine(LoadScene(_sceneTransitionManager.LoadedSceneName));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return new WaitForSeconds(0.4f);
            _sceneTransitionManager.LoadScene(sceneName);
        }
    }
}
