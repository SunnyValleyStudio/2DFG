using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.StaminaSystem
{
    public class UIStamina : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _staminaVal;
        public void UpdateStamina(int current, int max)
        {
            _staminaVal.text = $"{current} / {max}";
        }
    }
}
