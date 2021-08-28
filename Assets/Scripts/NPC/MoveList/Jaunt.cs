using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Jaunt : State
    {
        private MoveLibrary.Jaunt jaunt;
        private PathfindingHandler pathHandler;
        // private float timer = 0;

        public Jaunt(Controller controller) : base(controller)
        {
            jaunt = (MoveLibrary.Jaunt)Controller.CurrentMove;
            pathHandler = Controller.GetComponent<PathfindingHandler>();
            pathHandler.speed = jaunt.speed;
            pathHandler.SetTarget(jaunt.target);
            Debug.Log("Jaunt Triggered");
        }

        public override void EnterState()
        {
        }

        public override void Update()
        {
            pathHandler.HandleMovement();
            if (Controller.IsStaggered)
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
    }
}