using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Farming
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

		[SerializeField]
		private FieldPositionValidator _fieldPositionValidator;

        private void Awake()
        {
            _fieldPositionValidator = FindObjectOfType<FieldPositionValidator>();
			if (_fieldPositionValidator == null)
				Debug.LogWarning("Field positon will not be validated without Field Position Validator"
					, gameObject);
        }

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
				if(_fieldPositionValidator != null && _fieldPositionValidator.IsItFieldTile(PositionInFront))
				{
					Vector2 validPosition = _fieldPositionValidator.GetValidFieldTile(PositionInFront);
					Gizmos.DrawWireCube(validPosition, Vector2.one);
				}
			}
        }

        public List<Vector2> DetectValidTiles()
        {
			if (_fieldPositionValidator == null)
				return new List<Vector2>();
			return _fieldPositionValidator.GetValidFieldTiles(new List<Vector2>() { PositionInFront } );
        }
    }
}
