using System;
using System.Collections.Generic;
using System.Linq;
using fsm.states;
using UnityEngine;

namespace fsm
{
    public class StateMachine : MonoBehaviour
    {
        private Dictionary<Type, BaseState> _availableStates;

        private BaseState _currentState;
        public BaseState CurrentState => _currentState;

        public event Action<BaseState> OnStateChanged; 
        
        public void SetStates(Dictionary<Type, BaseState> states)
        {
            _availableStates = states;
        }

        private void Update()
        {
            if (_currentState == null)
            {
                _currentState = _availableStates.Values.First();
                _currentState.EnterState();
            }

            Type nextState = _currentState?.Tick();
            if (nextState != null &&
                nextState != _currentState?.GetType())
            {
                SwitchToNewState(nextState);
            }
        }

        private void SwitchToNewState(Type nextState)
        {
            _currentState.ExitState();
            
            _currentState = _availableStates[nextState];
            OnStateChanged?.Invoke(_currentState);
            
            _currentState.EnterState();
        }
    }
}