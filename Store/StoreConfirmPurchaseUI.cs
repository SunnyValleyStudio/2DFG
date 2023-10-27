using FarmGame.DataStorage;
using FarmGame.Input;
using System;
using UnityEngine;

namespace FarmGame.Store
{
    public class StoreConfirmPurchaseUI : MonoBehaviour
    {
        public int Quantity { get; internal set; }

        internal void HideConfirmWindow()
        {
            throw new NotImplementedException();
        }

        internal void ShowConfirmUI(PlayerInputFarm input, ItemDescription description, int money)
        {
            throw new NotImplementedException();
        }
    }
}