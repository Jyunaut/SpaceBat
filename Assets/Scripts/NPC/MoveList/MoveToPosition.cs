using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class MoveToPosition : State
    {
        private MoveLibrary.MoveToPosition moveToPosition;
        private Vector2 targetPos;

        public MoveToPosition(Controller controller) : base(controller)
        {
            moveToPosition = (MoveLibrary.MoveToPosition)Controller.CurrentMove;
            Debug.Log(Controller.SpawnPosition);
            targetPos = Controller.SpawnPosition + moveToPosition.position;
        }

        public override void Update()
        {
            Controller.transform.position = Vector2.Lerp(Controller.transform.position, targetPos, Time.deltaTime);
        }

        public override void Transitions()
        {
            if ((targetPos - (Vector2)Controller.transform.position).sqrMagnitude <= 0.1f)
            {
                Controller.TriggeredOnMoveComplete();
            }
        }
    }
}