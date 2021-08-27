using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Runner : State
    {
        private MoveLibrary.Runner runner;
        private PathfindingHandler pathHandler;
        private float timer = 0;

        public Runner(Controller controller) : base(controller)
        {
            runner = (MoveLibrary.Runner)Controller.currentMove;
            pathHandler = Controller.GetComponent<PathfindingHandler>();
            pathHandler.speed = runner.speed;
            pathHandler.SetTarget(GetTarget());
            Debug.Log("Runner Triggered");
        }

        public override void Update()
        {
            Debug.Log(pathHandler.isReached);
            if(!Controller.IsStaggered)
            {
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
                pathHandler.SetTarget(GetTarget());
                // Controller.TriggeredOnMoveComplete();
            }
            // else if (moveToPlayer.animation != null && timer >= moveToPlayer.animation.clip.length)
            //     Controller.TriggeredOnMoveComplete();
            // timer += Time.deltaTime;
        }

        private Vector3 GetTarget()
        {
            Pathfinding.Instance.grid.GetXY(Controller.transform.position, out int x, out int y);
            x = Mathf.Abs(x) + Mathf.Abs(runner.range) * (int)runner.direction.x;
            y = Mathf.Abs(y) + Mathf.Abs(runner.range) * (int)runner.direction.y;

            return Pathfinding.Instance.grid.GetWorldPosition(x, y); ;
        }
    }
}