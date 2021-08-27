using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class MoveToPosition : State
    {
        private MoveLibrary.MoveToPosition moveToPosition;
        private Coroutine action;
        private Vector2 initPos;

        public MoveToPosition(Controller controller) : base(controller)
        {
            moveToPosition = (MoveLibrary.MoveToPosition)Controller.CurrentMove;
            initPos = Controller.transform.position;
        }

        public override void EnterState()
        {
            action = Controller.StartCoroutine(Move());
        }

        public override void ExitState()
        {
            Controller.StopCoroutine(action);
        }

        private IEnumerator Move()
        {
            while (((Vector2)Controller.transform.position - (initPos + moveToPosition.position)).sqrMagnitude >= 0.1f)
            {
                Vector2 direction = (Vector2)Controller.transform.position - (initPos + moveToPosition.position);
                Controller.Rigidbody2D.MovePosition((Vector2)Controller.transform.position + moveToPosition.speed * direction * Time.deltaTime);
                yield return null;
            }
        }
    }
}