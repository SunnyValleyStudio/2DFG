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
			private set { 
				_isNearField = value; 
				if(_isNearField == false)
				{
					_validSelectionPositions = new();
					OnFieldExited?.Invoke();
				}
			}
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
        public event Action OnFieldExited;
        public event Action OnResetDetectedFields;
        public event Action<IEnumerable<Vector2>> OnPositionsDetected;

		[SerializeField]
		private float _checkDelay = 0.01f;
		Coroutine _oldCoroutine = null;
		private List<Vector2> _validSelectionPositions = new();

		public List<Vector2> ValidSelectionPositions
		{
			get { return _validSelectionPositions; }
		}

		private void Awake()
        {
            _fieldPositionValidator = FindObjectOfType<FieldPositionValidator>();
			if (_fieldPositionValidator == null)
				Debug.LogWarning("Field positon will not be validated without Field Position Validator"
					, gameObject);
        }

		public void StartChecking(Vector2Int detectionRange)
		{
			StopChecking();
			_oldCoroutine = StartCoroutine(CheckField(detectionRange));

        }
        public void StopChecking()
        {
            if(_oldCoroutine != null )
				StopCoroutine(_oldCoroutine);
        }

        private IEnumerator CheckField(Vector2Int detectionRange)
        {
            if(_isNearField && 
				_fieldPositionValidator != null &&
				_fieldPositionValidator.IsItFieldTile(PositionInFront))
			{
				_validSelectionPositions = DetectValidTiles(detectionRange);
				OnPositionsDetected?.Invoke(ValidSelectionPositions);
			}
			else
			{
				_validSelectionPositions.Clear();
				OnResetDetectedFields?.Invoke();
			}
			yield return new WaitForSeconds(_checkDelay);
			_oldCoroutine = StartCoroutine(CheckField(detectionRange));
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
				IsNearField = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(_fieldTag))
                IsNearField = false;
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

		//0x0 -> 1 tile in front
		//1x1 -> 3x3 area
		//2x2 -> 5x5 area
        public List<Vector2> DetectValidTiles(Vector2Int detectionRange)
        {
			if (_fieldPositionValidator == null)
				return new List<Vector2>();

			int halfX = detectionRange.x;
			int halfY = detectionRange.y;
			int xMax = halfX * 2 + 1;
			int yMax = halfY * 2 + 1;

			List<Vector2> tilesToCheck = new();
			Vector2 positionInFrontCached = PositionInFront;
			for (int x = 0; x < xMax; x++)
			{
				for (int y = 0; y < yMax; y++)
				{
					tilesToCheck.Add(positionInFrontCached + new Vector2(x - halfX, y - halfY));
				}
			}
			return _fieldPositionValidator.GetValidFieldTiles(tilesToCheck);
			//return _fieldPositionValidator.GetValidFieldTiles(new List<Vector2>() { PositionInFront } );
        }
    }
}
