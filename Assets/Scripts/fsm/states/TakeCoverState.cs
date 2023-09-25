using System;
using characters;
using core;
using fsm.animation;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace fsm.states
{
    public class TakeCoverState : BaseState
    {
        private readonly BadGuy _badGuy;
        private readonly BadGuyAnimator _badGuyAnimator;

        private readonly Vector3 _initialPosition;
        private readonly Quaternion _initialRotation;
        
        public TakeCoverState(BadGuy badGuy, BadGuyAnimator badGuyAnimator, Vector3 initialPosition, Quaternion initialRotation) : base(badGuy.gameObject)
        {
            _badGuy = badGuy;
            _badGuyAnimator = badGuyAnimator;

            _initialPosition = initialPosition;
            _initialRotation = initialRotation;
        }

        public override void EnterState()
        {
            _badGuyAnimator.AnimateTakeCoverState();
        }

        public override Type Tick()
        {
            if (_badGuy.IsDead)
                return typeof(DeadState);
            
            Transform chaseTarget = CheckForAggro();
            if (chaseTarget != null)
            {
                _badGuy.SetTarget(chaseTarget);
                return typeof(ChaseState);
            }

            if (Vector3.Distance(_badGuy.transform.position, _initialPosition) > GameSettings.ReturnedToCoverDistance)
            {
                Vector3 targetDirection = _initialPosition - _badGuy.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                _transform.rotation = Quaternion.Lerp(_transform.rotation, targetRotation, 0.2f);
                _transform.Translate(Vector3.forward * (Time.deltaTime * GameSettings.BadGuyChaseSpeed));
            }

            return null;
        }
        
        
        private readonly Quaternion _startingAngle = Quaternion.AngleAxis(-80, Vector3.up);
        private readonly Quaternion _stepAngle = Quaternion.AngleAxis(4, Vector3.up);
        
        private Transform CheckForAggro()
        {
            float aggroRadius = GameSettings.AggroRadius;

            var angle = _transform.rotation * _startingAngle;
            var direction = angle * Vector3.forward;
            var pos = _transform.position;
            for (var i = 0; i < 40; i++)
            {
                RaycastHit[] hits = Physics.RaycastAll(pos, direction, aggroRadius);
                if (hits != null && hits.Length > 0)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        BadGuy badGuy = hit.collider.GetComponent<BadGuy>();
                        if (badGuy != null && badGuy.Team != _gameObject.GetComponent<BadGuy>().Team && !badGuy.IsDead)
                        {
                            Debug.DrawRay(pos, direction * hit.distance, Color.red);
                            return badGuy.transform;
                        }
                            
                        Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                    }
                }
                else
                {
                    Debug.DrawRay(pos, direction * aggroRadius, Color.white);
                }

                direction = _stepAngle * direction;
            }

            return null;
        }

        public override string GetStateName()
        {
            return $"{_badGuy.gameObject.name} is wandering.";
        }
    }
}