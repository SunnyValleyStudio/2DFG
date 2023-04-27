using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Agent
{
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField]
        private ContactFilter2D _contactFilter;

        private RaycastHit2D[] _hitObjects = new RaycastHit2D[8];

        [SerializeField]
        private Collider2D _movementCollider;

        [SerializeField]
        private float _safetyCollisionOffset = 0.01f;

        [SerializeField]
        private int _collisionResult = 0;

        private void Awake()
        {
            Debug.Assert(_movementCollider != null, "Collider can't be null", gameObject);
        }

        public bool IsMovementValid(Vector2 movementDirection, float distanceToMoveThisFrame)
        {
            _collisionResult = _movementCollider.Cast(
                movementDirection,
                _contactFilter,
                _hitObjects,
                distanceToMoveThisFrame + _safetyCollisionOffset);

            Debug.DrawRay(transform.position + (Vector3)_movementCollider.offset,
                movementDirection*(distanceToMoveThisFrame+_safetyCollisionOffset),
                _collisionResult == 0 ? Color.green : Color.red);

            return _collisionResult == 0;
        }
    }
}
