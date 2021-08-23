using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class State : StateMachine
    {
        public bool IsDoingAction { get; set; }

        protected Controller Controller { get; set; }
        
        protected static Coroutine actionCoroutine;

        public State(Controller controller)
        {
            Controller = controller;
        }

        public override void EnterState()
        {
            Controller.Animator.Play(nameof(Controller.State));
        }
    }
    class Idle : State
    {
        public Idle(Controller controller) : base(controller) {}
    }
    class Action : State
    {
        public Action(Controller controller) : base(controller) {}

        public override void EnterState()
        {
            base.EnterState();
            // DoAction(Controller.Animator.);
        }

        public override void Transitions()
        {

        }

        private void DoAction(float duration)
        {
            if (actionCoroutine != null)
            {
                Controller.StopCoroutine(actionCoroutine);
                actionCoroutine = null;
            }
            actionCoroutine = Controller.StartCoroutine(StartAction(duration));

            IEnumerator StartAction(float duration)
            {
                // float time = 0;
                yield return null;
            }
        }
    }
}