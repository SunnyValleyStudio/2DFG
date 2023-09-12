using FarmGame.Agent;
using FarmGame.SaveSystem;
using FarmGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.SceneTransitions
{
    public class SceneTransitionSpawner : MonoBehaviour, ISavable
    {
        private GameObject player;

        private SceneTransitionTrigger[] _transitionTriggers;

        public int SaveID => SaveIDRepositor.SCENE_TRANSITION_SPAWNER;

        private void Awake()
        {
            Player p;
            if(p = FindObjectOfType<Player>())
            {
                player = p.gameObject;
            }
            else
            {
                Debug.LogError($"Player couldn't be found", gameObject);
            }
            _transitionTriggers = FindObjectsOfType<SceneTransitionTrigger>();
        }

        public string GetData()
        {
            SaveData saveData = new();
            foreach(SceneTransitionTrigger trigger in _transitionTriggers)
            {
                if (trigger.Triggered)
                {
                    saveData = new()
                    {
                        TransitionID = trigger.SceneTriggerID
                    };
                    break;
                }
            }
            return JsonUtility.ToJson(saveData);
        }

        public void RestoreData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return;
            }
            SaveData dataToLoad = JsonUtility.FromJson<SaveData>(data);
            if(dataToLoad.TransitionID >= 0)
            {
                foreach (var item in _transitionTriggers)
                {
                    if(item.SceneTriggerID == dataToLoad.TransitionID)
                    {
                        player.transform.position = item.SpawnPoint.position;
                        ScreenTransitionEffect effect 
                            = FindObjectOfType<ScreenTransitionEffect>();
                        effect.PlayTransition(true);
                        return;
                    }
                }
                Debug.LogWarning($"No Transition with id {dataToLoad.TransitionID}", gameObject);
            }

        }

        [Serializable]
        public struct SaveData
        {
            public int TransitionID;
        }
    }
}
