using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame
{
    public class FieldColliderTest : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision.gameObject.name + " Entering");
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log(collision.gameObject.name + " EXITING");
        }
    }
}
