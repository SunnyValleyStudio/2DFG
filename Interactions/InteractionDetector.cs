using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Interactions
{
    public class InteractionDetector : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _interactionLayerMask;
        [SerializeField, Range(0.1f,1)]
        private float _interactionDistance = 0.5f;

        private Vector2 _interactionDirection;

        public void SetInteractionDirection(Vector2 input)
        {
            if(input.magnitude > 0.1f)
                _interactionDirection = input.normalized;
        }

        public IEnumerable<IInteractable> PerformDetection()
        {
            Collider2D collisionResult 
                = Physics2D.OverlapCircle(
                    (Vector2)transform.position + _interactionDirection*_interactionDistance,
                    0.1f,
                    _interactionLayerMask);
            if(collisionResult != null)
            {
                return collisionResult.GetComponents<IInteractable>();
            }
            return new List<IInteractable>();
        }
    }
}
