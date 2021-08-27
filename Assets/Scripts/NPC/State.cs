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
                Debug.Log(Controller.State.ToString() + " Triggered");
                yield return new WaitForSeconds(duration);
                Debug.Log(Controller.State.ToString() + " Complete");
                Controller.TriggeredOnMoveComplete();
            }
        }

        public void Wait() => Controller.SetState(new Wait(Controller));
        public void Shoot() => Controller.SetState(new Shoot(Controller));
        public void Jaunt() => Controller.SetState(new Jaunt(Controller));
        public void MoveToPlayer() => Controller.SetState(new MoveToPlayer(Controller));
        public void Runner() => Controller.SetState(new Runner(Controller));
        public void Explode() => Controller.SetState(new Explode(Controller));
        public void Shotgun() => Controller.SetState(new Shotgun(Controller));
        public void Melee() => Controller.SetState(new Melee(Controller));
        public void Laser() => Controller.SetState(new Laser(Controller));
        public void JauntGun() => Controller.SetState(new JauntGun(Controller));
    }
    class Hurt : State
    {
        private float _hurtTime;

        public Hurt(Controller controller) : base(controller)
        {
            _hurtTime = Time.time + 1f;
        }

        public override void Transitions()
        {
            if (Time.time >= _hurtTime)
            {
                Controller.TriggeredOnMoveComplete();
            }
        }
    }
}
