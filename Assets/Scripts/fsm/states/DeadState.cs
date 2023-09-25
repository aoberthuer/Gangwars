using System;
using characters;
using fsm.animation;
using UnityEngine;

namespace fsm.states
{
    public class DeadState : BaseState
    {
        private readonly BadGuy _badGuy;
        private BadGuyAnimator _badGuyAnimator;

        public DeadState(BadGuy badGuy, BadGuyAnimator badGuyAnimator) : base(badGuy.gameObject)
        {
            _badGuy = badGuy;
            _badGuyAnimator = badGuyAnimator;
        }

        public override void EnterState()
        {
            _badGuyAnimator.AnimateDeadState();
        }

        public override Type Tick()
        {
            // do nothing 
            return null;
        }

        public override string GetStateName()
        {
            return $"{_badGuy.gameObject.name} is dead.";
        }
    }
}