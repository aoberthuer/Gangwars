using System;
using characters;
using core;
using fsm.animation;
using UnityEngine;

namespace fsm.states
{
    public class ChaseState : BaseState
    {
        private readonly BadGuy _badGuy;
        private readonly BadGuyAnimator _badGuyAnimator;
        
        private readonly AttackStrategy _attackStrategy;

        public ChaseState(BadGuy badGuy, BadGuyAnimator badGuyAnimator, AttackStrategy attackStrategy) : base(badGuy.gameObject)
        {
            _badGuy = badGuy;
            _badGuyAnimator = badGuyAnimator;

            _attackStrategy = attackStrategy;
        }

        public override void EnterState()
        {
            _badGuyAnimator.AnimateChaseState();
        }

        public override Type Tick()
        {
            if (_badGuy.IsDead)
                return typeof(DeadState);

            if (_badGuy.Target == null || _badGuy.IsDead)
            {
                if(_attackStrategy == AttackStrategy.ChaseAndAttack)
                    return typeof(WanderState);
                
                if (_attackStrategy == AttackStrategy.TakeCoverAndAttack)
                    return typeof(TakeCoverState);
            }

            _transform.LookAt(_badGuy.Target);
            _transform.Translate(Vector3.forward * (Time.deltaTime * GameSettings.BadGuyChaseSpeed));

            float distanceToTarget = Vector3.Distance(_transform.position, _badGuy.Target.position);
            if (distanceToTarget <= GameSettings.AttackRange)
            {
                Debug.Log($"Distance to target is {distanceToTarget}");
                return typeof(AttackState);
            }

            return null;
        }

        public override string GetStateName()
        {
            return $"{_badGuy.gameObject.name} is chasing.";
        }
    }
}