using System;
using characters;
using fsm.animation;
using UnityEngine;

namespace fsm.states
{
    public class AttackState : BaseState
    {
        private readonly BadGuy _badGuy;
        private readonly BadGuyAnimator _badGuyAnimator;
        
        private float _attackReadyTimer = 0.5f;
        public AttackState(BadGuy badGuy, BadGuyAnimator badGuyAnimator) : base(badGuy.gameObject)
        {
            _badGuy = badGuy;
            _badGuyAnimator = badGuyAnimator;
        }
        
        public override void EnterState()
        {
            _badGuyAnimator.AnimateAttackState();
        }

        public override Type Tick()
        {
            if(_badGuy.IsDead)
                return typeof(DeadState);
            
            if (_badGuy.Target == null || _badGuy.Target.GetComponent<BadGuy>().IsDead)
                return typeof(WanderState);

            _attackReadyTimer -= Time.deltaTime;
            if (_attackReadyTimer <= 0f)
            {
                Debug.Log("Attack!");
                
                _badGuy.FireWeapon();
                _attackReadyTimer = 1f;
            }

            return null;
        }

        public override string GetStateName()
        {
            return $"{_badGuy.gameObject.name} is attacking.";
        }
    }
}