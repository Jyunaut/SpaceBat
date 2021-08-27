using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Shoot : State
    {
        private Coroutine coroutine;
        private MoveLibrary.Shoot shoot;

        public Shoot(Controller controller) : base(controller)
        {
            shoot = (MoveLibrary.Shoot)Controller.CurrentMove;
        }

        public override void EnterState()
        {
            GameObject.Instantiate(shoot.bullet, Controller.transform.position, Quaternion.identity);
            Duration(Controller.CurrentMove.duration, ref coroutine);
        }
    }
}