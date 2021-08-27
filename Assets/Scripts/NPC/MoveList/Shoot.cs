using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Shoot : State
    {
        private Coroutine coroutine;
        private MoveLibrary.Shoot shoot;
        private Vector3 direction;

        public Shoot(Controller controller) : base(controller)
        {
            shoot = (MoveLibrary.Shoot)Controller.CurrentMove;
            direction = GameManager.Instance.player.transform.position - Controller.transform.position;
            direction = direction.normalized;
        }

        public override void EnterState()
        {
            GameObject bullet = Controller.Instantiate(shoot.bullet, Controller.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = shoot.speed * direction;
            Duration(Controller.CurrentMove.duration, ref coroutine);
        }
    }
}