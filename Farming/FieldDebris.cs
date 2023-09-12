using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Farming
{
    public class FieldDebris : MonoBehaviour
    {
        private FieldController _fieldController;
        private void Awake()
        {
            _fieldController = FindObjectOfType<FieldController>();
        }

        private void Start()
        {
            _fieldController.AddDebrisAt(transform.position, gameObject);
        }

        public void RemoveRebirs()
        {
            _fieldController.RemoveDebris(transform.position);
        }
    }
}
