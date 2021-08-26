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

    }
    class Wait : State
    {
        private Coroutine coroutine;
        private MoveLibrary.Wait wait;

        public Wait(Controller controller) : base(controller)
        {
            wait = (MoveLibrary.Wait)Controller.currentMove;
        }

        public override void EnterState()
        {
            Duration(wait.duration, ref coroutine);
        }
    }
    class Shoot : State
    {
        private Coroutine coroutine;
        private MoveLibrary.Shoot shoot;

        public Shoot(Controller controller) : base(controller)
        {
            shoot = (MoveLibrary.Shoot)Controller.currentMove;
        }

        public override void EnterState()
        {
            GameObject.Instantiate(shoot.bullet, Controller.transform.position, Quaternion.identity);
            Duration(Controller.currentMove.duration, ref coroutine);
        }
    }
    class Jaunt : State
    {
        private Coroutine coroutine;
        private MoveLibrary.Jaunt jaunt;
        private PathfindingHandler pathHandler;

        public Jaunt(Controller controller) : base(controller)
        {
            jaunt = (MoveLibrary.Jaunt)Controller.currentMove;
            pathHandler = Controller.GetComponent<PathfindingHandler>();
            pathHandler.speed = jaunt.speed;
            pathHandler.isReached = false;
            Debug.Log("Jaunt Triggered");
        }

        public override void EnterState()
        {
            pathHandler.SetTarget(jaunt.target);
        }

        public override void Update()
        {
            if (pathHandler.isReached)
            {
                Debug.Log(Controller.state.ToString() + " Complete");
                pathHandler.isReached = false;
                Controller.TriggeredOnMoveComplete();
            }
        }
    }
    class MoveToTarget : State
    {
        private MoveLibrary.MoveToTarget moveToTarget;
        private PathfindingHandler pathHandler;
        private GameObject player;
        private float timer;

        public MoveToTarget(Controller controller) : base(controller)
        {
            moveToTarget = (MoveLibrary.MoveToTarget)Controller.currentMove;
            pathHandler = Controller.GetComponent<PathfindingHandler>();
            pathHandler.speed = moveToTarget.speed;
            player = GameObject.FindGameObjectWithTag(GlobalStrings.kPlayer);
            pathHandler.SetTarget(player.transform.position);
            timer = 0;
            Debug.Log("Kamikaze");
        }

        public override void Update()
        {
            pathHandler.SetTarget(player.transform.position);
        }

        public override void Transitions()
        {
            if (pathHandler.isReached)
            {
                pathHandler.isReached = false;
                Controller.TriggeredOnMoveComplete();
            }
            else if (timer >= moveToTarget.animation.clip.length)
                Controller.TriggeredOnMoveComplete();
            timer += Time.deltaTime;
        }
    }
}
