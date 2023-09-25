using System;
using UnityEngine;

namespace fsm.states
{
    public abstract class BaseState
    {
        protected GameObject _gameObject;
        protected Transform _transform;
        
        public BaseState(GameObject gameObject)
        {
            _gameObject = gameObject;
            _transform = gameObject.transform;
        }

        public abstract Type Tick();

        public abstract string GetStateName(); 

        public virtual void ExitState()
        {
        }

        public virtual void EnterState()
        {
        }
    }
}