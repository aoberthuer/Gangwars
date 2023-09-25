using System;
using System.Collections.Generic;
using fsm;
using fsm.animation;
using fsm.states;
using projectile;
using UnityEngine;

namespace characters
{
    public class BadGuy : MonoBehaviour
    {
        [SerializeField] private Team _team;
        public Team Team => _team;

        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Transform _projectilePrefabLaunchTransform;

        [SerializeField] private Light _flashLight;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClipGunShot;

        private Transform _target;
        public Transform Target => _target;

        private StateMachine _stateMachine;
        private BadGuyAnimator _badGuyAnimator;

        private bool _isDead;
        public bool IsDead => _isDead;

        private void Awake()
        {
            _stateMachine = GetComponent<StateMachine>();
            _badGuyAnimator = GetComponent<BadGuyAnimator>();

            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
            {
                {typeof(WanderState), new WanderState(this, _badGuyAnimator)},
                {typeof(ChaseState), new ChaseState(this, _badGuyAnimator)},
                {typeof(AttackState), new AttackState(this, _badGuyAnimator)},
                {typeof(DeadState), new DeadState(this, _badGuyAnimator)}
            };

            _stateMachine.SetStates(states);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void Kill()
        {
            _isDead = true;
            _flashLight.gameObject.SetActive(false);
        }

        public void FireWeapon()
        {
            Projectile projectileInstance = Instantiate(_projectilePrefab, 
                _projectilePrefabLaunchTransform.position,
                Quaternion.identity);
            projectileInstance.SetTarget(_target.GetComponent<BadGuy>());
            
            _audioSource.PlayOneShot(_audioClipGunShot);
        }
    }
}