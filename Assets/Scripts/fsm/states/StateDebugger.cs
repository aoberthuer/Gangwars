using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace fsm.states
{
    public class StateDebugger : MonoBehaviour
    {

        private List<StateMachine> _stateMachines;

        private void Awake()
        {
            _stateMachines = FindObjectsByType<StateMachine>(FindObjectsSortMode.None).ToList();
        }

        private void OnEnable()
        {
            foreach (StateMachine stateMachine in _stateMachines)
            {
                stateMachine.OnStateChanged += HandleStateChange;
            }
        }

        private void HandleStateChange(BaseState changedState)
        {
            Debug.Log($"State has changed to {changedState}");
        }
    }
}