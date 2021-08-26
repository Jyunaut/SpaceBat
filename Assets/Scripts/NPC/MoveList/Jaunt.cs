using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
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
}