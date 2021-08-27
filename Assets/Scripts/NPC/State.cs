using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using MoveLibrary;

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
        public void Jaunt() => Controller.SetState(new Jaunt(Controller));
        public void MoveToTarget() => Controller.SetState(new MoveToTarget(Controller));
        public void Runner() => Controller.SetState(new Runner(Controller));

    }
}
