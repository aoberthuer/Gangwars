using System;
using characters;
using core;
using fsm.animation;
using UnityEngine;

namespace fsm.states
{
    public class WanderState : BaseState
    {
        private readonly BadGuy _badGuy;
        private readonly BadGuyAnimator _badGuyAnimator;

        private readonly LayerMask _layerMask = LayerMask.GetMask("Walls");

        private readonly float _stopDistance = 1.5f;
        private readonly float _turnSpeed = 1f;

        private Vector3? _destination;
        private Quaternion _desiredRotation;
        private Vector3 _direction;

        public WanderState(BadGuy badGuy, BadGuyAnimator badGuyAnimator) : base(badGuy.gameObject)
        {
            _badGuy = badGuy;
            _badGuyAnimator = badGuyAnimator;
        }

        public override void EnterState()
        {
            _badGuyAnimator.AnimateWanderState();
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

            if (_destination.HasValue == false ||
                Vector3.Distance(_transform.position, _destination.Value) <= _stopDistance)
            {
                FindRandomDestination();
            }

            _transform.rotation = Quaternion.Slerp(_transform.rotation, _desiredRotation, Time.deltaTime * _turnSpeed);

            if (IsForwardBlocked())
            {
                _transform.rotation = Quaternion.Lerp(_transform.rotation, _desiredRotation, 0.2f);
            }
            else
            {
                _transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.BadGuyWanderSpeed);
            }

            Debug.DrawRay(_transform.position, _direction * GameSettings.ForwardLookAhead, Color.red);
            while (IsPathBlocked())
            {
                FindRandomDestination();
                Debug.Log("WALL!");
            }

            return null;
        }

        private readonly Quaternion _startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
        private readonly Quaternion _stepAngle = Quaternion.AngleAxis(3, Vector3.up);

        private Transform CheckForAggro()
        {
            float aggroRadius = GameSettings.AggroRadius;

            RaycastHit hit;
            var angle = _transform.rotation * _startingAngle;
            var direction = angle * Vector3.forward;
            var pos = _transform.position;
            for (var i = 0; i < 40; i++)
            {
                if (Physics.Raycast(pos, direction, out hit, aggroRadius))
                {
                    var badGuy = hit.collider.GetComponent<BadGuy>();
                    if (badGuy != null && badGuy.Team != _gameObject.GetComponent<BadGuy>().Team && !badGuy.IsDead)
                    {
                        Debug.DrawRay(pos, direction * hit.distance, Color.red);
                        return badGuy.transform;
                    }

                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                }
                else
                {
                    Debug.DrawRay(pos, direction * aggroRadius, Color.white);
                }

                direction = _stepAngle * direction;
            }

            return null;
        }

        private bool IsForwardBlocked()
        {
            Ray ray = new Ray(_transform.position, _transform.forward);
            return Physics.SphereCast(ray, 0.5f, GameSettings.ForwardLookAhead, _layerMask);
        }

        private bool IsPathBlocked()
        {
            Ray ray = new Ray(_transform.position, _direction);
            bool pathBlocked = Physics.SphereCast(ray, 0.5f, GameSettings.ForwardLookAhead, _layerMask);

            Debug.Log($"Path blocked for {_gameObject.name}: {pathBlocked}");
            return pathBlocked;
        }

        private void FindRandomDestination()
        {
            Vector3 testPosition = (_transform.position +
                                    (_transform.forward * 4f) +
                                    new Vector3(
                                        UnityEngine.Random.Range(-4.5f, 4.5f),
                                        0f,
                                        UnityEngine.Random.Range(-4.5f, 4.5f)));

            _destination = new Vector3(testPosition.x, 1f, testPosition.z);

            _direction = Vector3.Normalize(_destination.Value - _transform.position);
            _direction = new Vector3(_direction.x, 0f, _direction.z);

            _desiredRotation = Quaternion.LookRotation(_direction);
            Debug.Log("Got direction.");
        }

        public override string GetStateName()
        {
            return $"{_badGuy.gameObject.name} is wandering.";
        }
    }
}