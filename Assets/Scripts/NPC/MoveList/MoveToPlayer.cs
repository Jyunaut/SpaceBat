using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class MoveToTarget : State
    {
        private MoveLibrary.MoveToPlayer moveToPlayer;
        private PathfindingHandler pathHandler;
        private GameObject player;
        private float timer;

        public MoveToTarget(Controller controller) : base(controller)
        {
            moveToPlayer = (MoveLibrary.MoveToPlayer)Controller.CurrentMove;
            pathHandler = Controller.GetComponent<PathfindingHandler>();
            pathHandler.speed = moveToPlayer.speed;
            player = GameManager.Instance.player;
            timer = 0;
            Debug.Log("Kamikaze");
        }

        public override void FixedUpdate()
        {
            Debug.Log(Controller.IsStaggered);
            if (!Controller.IsStaggered)
            {
                pathHandler.SetTarget(player.transform.position);
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
            else if (moveToPlayer.animation != null && timer >= moveToPlayer.animation.clip.length)
                Controller.TriggeredOnMoveComplete();
            timer += Time.deltaTime;
        }
    }
}
