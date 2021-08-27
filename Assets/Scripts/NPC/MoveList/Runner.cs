using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Runner : State
    {
        private MoveLibrary.Runner runner;
        private PathfindingHandler pathHandler;
        private Vector3 direction;
        private float timer = 0;

        public Runner(Controller controller) : base(controller)
        {
            runner = (MoveLibrary.Runner)Controller.currentMove;
            pathHandler = Controller.GetComponent<PathfindingHandler>();
            pathHandler.speed = runner.speed;
            Debug.Log("Jaunt Triggered");
        }

        public override void EnterState()
        {
        }

        public override void Update()
        {
            Debug.Log(Controller.IsStaggered);
            if(!Controller.IsStaggered)
            {
                pathHandler.SetTarget(GetTarget());
                pathHandler.HandleMovement();
            }
            else
            {
                pathHandler.StopMoving();
            }
        }

        public override void Transitions()
        {
            if (pathHandler.isReached)
            {
                pathHandler.isReached = false;
                Controller.TriggeredOnMoveComplete();
            }
            // else if (moveToPlayer.animation != null && timer >= moveToPlayer.animation.clip.length)
            //     Controller.TriggeredOnMoveComplete();
            // timer += Time.deltaTime;
        }

        private Vector3 GetTarget()
        {
            return Vector3.zero;
        }
    }
}