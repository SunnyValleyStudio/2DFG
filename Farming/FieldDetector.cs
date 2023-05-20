using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame
{
    public class FieldDetector : MonoBehaviour
    {
		private bool _isNearField;

		public bool IsNearField
		{
			get { return _isNearField; }
			set { _isNearField = value; }
		}

		[SerializeField]
		private string _fieldTag = "Field";
		[SerializeField]
		private Transform _interactorCenter;
		private Vector2 _interactionDirection;

		public Vector2 PositionInFront => 
			(Vector2)_interactorCenter.position + _interactionDirection * 0.5f;

		public void SetInteractionDirection(Vector2 direction)
		{
			if(direction.magnitude > 0.1f)
			{
				_interactionDirection = direction;
			}
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag(_fieldTag))
				_isNearField = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(_fieldTag))
                _isNearField = false;
        }

        private void OnDrawGizmosSelected()
        {
            if(Application.isPlaying && _isNearField)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(PositionInFront, 0.2f);
			}
        }
    }
}
