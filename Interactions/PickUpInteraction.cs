using FarmGame.Agent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Interactions
{
    public class PickUpInteraction : MonoBehaviour
    {
        public bool CanInteract()
            => true;
        public void Interact(Player agent)
        {
            Destroy(gameObject);
        }
    }
}
