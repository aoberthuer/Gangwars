using UnityEngine;

namespace fsm.animation
{
    public class BadGuyAnimator : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int VELOCITY = Animator.StringToHash("velocity");
        private static readonly int SHOOT = Animator.StringToHash("shoot");
        private static readonly int DIE = Animator.StringToHash("die");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void AnimateWanderState()
        {
            _animator.SetFloat(VELOCITY, 0.3f);
        }
        
        public void AnimateChaseState()
        {
            _animator.SetFloat(VELOCITY, 0.6f);
        }
        
        public void AnimateAttackState()
        {
            _animator.SetFloat(VELOCITY, 0f);
            _animator.SetTrigger(SHOOT);
        }

        public void AnimateDeadState()
        {
            _animator.SetFloat(VELOCITY, 0f);
            _animator.SetTrigger(DIE);
        }
    }
}