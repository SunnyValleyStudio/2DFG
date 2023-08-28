using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame
{
    [RequireComponent(typeof(Animator))]
    public class ToolAnimation : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField]
        private string directionX = "DirectionX",
            directionY = "DirectionY", useTrigger = "Use";

        [SerializeField, Min(0.01f)]
        private float _defaultAnimationSpeed = 1;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetAnimatorController(RuntimeAnimatorController animatorController)
        {
            _animator.runtimeAnimatorController = animatorController;
        }

        public void ChangeDirection(Vector2 direction)
        {
            if (direction.magnitude < 0.1f)
                return;
            Vector2Int directionInt = Vector2Int.RoundToInt(direction);
            if (directionInt.x != 0)
            {
                directionInt.y = 0;
            }
            _animator.SetFloat(directionX, directionInt.x);
            _animator.SetFloat(directionY, directionInt.y);
        }

        public void PlayAnimation(float animationSpeed = -1)
        {
            _animator.speed = animationSpeed <= 0 ? _defaultAnimationSpeed 
                : animationSpeed;
            _animator.SetTrigger(useTrigger);
        }
    }
}
