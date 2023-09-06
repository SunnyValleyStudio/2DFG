using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.SceneTransitions
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public event Action OnBeforeLoadScene;
        public void LoadScene(string sceneReferenceName)
        {
            OnBeforeLoadScene?.Invoke();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneReferenceName);
        }

    }
}
