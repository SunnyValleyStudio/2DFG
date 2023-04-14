using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Agent
{
    [RequireComponent(typeof(Animator))]
    public class AgentAnimation : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField]
        private string directionX = "DirectionX", directionY = "DirectionY", movingBoolFlag = "Movement";
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayMovementAnimation(bool val)
            => _animator.SetBool(movingBoolFlag, val);

        public void ChangeDirection(Vector2 direction)
        {
            if (direction.magnitude < 0.1f)
                return;
            Vector2Int directionInt = Vector2Int.RoundToInt(direction);
            _animator.SetFloat(directionX, directionInt.x);
            _animator.SetFloat(directionY, directionInt.y);
        }
    }
}
