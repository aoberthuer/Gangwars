﻿using UnityEngine;

namespace core
{
    public class GameSettings : MonoBehaviour
    {
        [SerializeField] private float _badGuyWanderSpeed = 2f;
        public static float BadGuyWanderSpeed => Instance._badGuyWanderSpeed;
        
        [SerializeField] private float _badGuyChaseSpeed = 4f;
        public static float BadGuyChaseSpeed => Instance._badGuyChaseSpeed;

        [SerializeField] private float _aggroRadius = 8f;
        public static float AggroRadius => Instance._aggroRadius;

        [SerializeField] private float _attackRange = 3f;
        public static float AttackRange => Instance._attackRange;
        
        [SerializeField] private float _forwardLookAhead = 5f;
        public static float ForwardLookAhead => Instance._forwardLookAhead;

        [SerializeField] private float _returnedToCoverDistance = 0.5f;
        public static double ReturnedToCoverDistance => Instance._returnedToCoverDistance;
        
        [SerializeField] private LayerMask _badGuysLayerMask;
        public static LayerMask BadGuysLayerMask => Instance._badGuysLayerMask;

        private static GameSettings Instance { get; set; }

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
    }
}