using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
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
}