using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace NPC
{
    public class State : StateMachine
    {
        public Controller Controller { get; set; }

        public State(Controller controller)
        {
            Controller = controller;
        }

        protected void Duration(float duration, ref Coroutine coroutine, bool startState = false) // TODO WHY HAVE START STATE
        {
            Controller.StartCoroutine(StartTimer(duration, startState));
            IEnumerator StartTimer(float duration, bool startState)
            {
                Debug.Log(Controller.state.ToString() + " Triggered");
                yield return new WaitForSeconds(duration);
                Debug.Log(Controller.state.ToString() + " Complete");
                Controller.TriggeredOnMoveComplete();
            }
        }

        public void Wait() => Controller.SetState(new Wait(Controller));
        public void Shoot() => Controller.SetState(new Shoot(Controller));

    }
    class Wait : State
    {
        private Coroutine coroutine;

        public Wait(Controller controller) : base(controller) { }

        public override void EnterState()
        {
            Duration(5f, ref coroutine);
        }
    }
    class Shoot : State
    {
        private Coroutine coroutine;

        public Shoot(Controller controller) : base(controller) { }

        public override void EnterState()
        {
            GameObject.Instantiate(Controller.currentMove.spawns[0], Controller.transform.position, Quaternion.identity);
            Duration(5f, ref coroutine);
        }
    }
}
