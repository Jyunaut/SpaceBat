using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class MoveToPlayer : State
    {
        private MoveLibrary.MoveToPlayer moveToPlayer;
        private PathfindingHandler pathHandler;
        private GameObject player;

        private const float checkInterval = 0.25f;
        private float timer;

        public MoveToPlayer(Controller controller) : base(controller)
        {
            moveToPlayer = (MoveLibrary.MoveToPlayer)Controller.CurrentMove;
            pathHandler = Controller.GetComponent<PathfindingHandler>();
            pathHandler.speed = moveToPlayer.speed;
            player = GameManager.Instance.player;
        }

        public override void FixedUpdate()
        {
            if (!Controller.IsStaggered)
            {
                pathHandler.SetTarget(player.transform.position);
                pathHandler.HandleMovement();
            }
        }

        public override void Transitions()
        {
            if (!moveToPlayer.followForever)
            {
                if (timer >= checkInterval)
                {
                    if ((player.transform.position - Controller.transform.position).magnitude <= moveToPlayer.stopDistance)
                    {
                        pathHandler.StopMoving();
                        Controller.TriggeredOnMoveComplete();
                    }
                    timer = 0f;
                }
                timer += Time.deltaTime;
            }
        }
    }
}
