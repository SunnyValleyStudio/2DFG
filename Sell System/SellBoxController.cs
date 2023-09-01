using FarmGame.DataStorage.Inventory;
using FarmGame.Input;
using FarmGame.TimeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.SellSystem
{
    public class SellBoxController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputFarm _input;

        [SerializeField]
        private GameObject _sellBoxCanvas;

        [SerializeField]
        private PauseTimeControllerSO _pauseTimeControllerSO;

        internal void PrepareSellBox(Inventory inventory)
        {
            _input.EnableUIActionMap();
            _input.OnUIExit += ExitUI;
            _sellBoxCanvas.SetActive(true);
            _pauseTimeControllerSO.SetTimePause(true);
        }

        private void ExitUI()
        {
            _sellBoxCanvas.SetActive(false);
            _pauseTimeControllerSO.SetTimePause(false);
            _input.EnableDefaultActionMap();
            _input.OnUIExit -= ExitUI;
        }
    }
}
