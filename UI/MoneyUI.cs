using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FarmGame.UI
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _moneyTxt;

        public void UpdateMoneyValue(int moneyAmount)
            => _moneyTxt.text = moneyAmount.ToString();
    }
}
