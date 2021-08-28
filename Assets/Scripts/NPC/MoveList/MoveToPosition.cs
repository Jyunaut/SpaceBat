using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class MoveToPosition : State
    {
        private MoveLibrary.MoveToPosition moveToPosition;
        private Vector2 startPos;
        private Vector2 targetPos;
        private float timer;

        public MoveToPosition(Controller controller) : base(controller)
        {
            moveToPosition = (MoveLibrary.MoveToPosition)Controller.CurrentMove;
            startPos = Controller.transform.position;
            targetPos = Controller.SpawnPosition + moveToPosition.position;
        }

        public override void Update()
        {
            Controller.transform.position = Vector2.Lerp(startPos, targetPos, timer / moveToPosition.travelTime);
            timer += Time.deltaTime;
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