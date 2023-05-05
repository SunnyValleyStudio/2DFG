using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Agent
{
    [RequireComponent(typeof(Animator))]
    public class AgentAnimation : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField]
        private string directionX = "DirectionX", 
            directionY = "DirectionY", 
            movingBoolFlag = "Movement",
            pickup = "Pickup";

        [HideInInspector]
        public UnityEvent OnAnimationEnd;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayerActionAnimationEnd()
        {
            OnAnimationEnd?.Invoke();
            OnAnimationEnd.RemoveAllListeners();
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

        public void PlayAnimation(AnimationType animationType)
        {
            if(animationType == AnimationType.PickUp)
            {
                _animator.SetTrigger(pickup);
            }
        }
    }

    public enum AnimationType
    {
        None,
        PickUp
    }
}
