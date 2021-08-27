using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class JauntGun : State
    {
        private MoveLibrary.JauntGun jauntGun;
        private PathfindingHandler pathHandler;
        private Vector3 direction;

        public JauntGun(Controller controller) : base(controller)
        {
            jauntGun = (MoveLibrary.JauntGun)Controller.CurrentMove;
            pathHandler = Controller.GetComponent<PathfindingHandler>();
            pathHandler.speed = jauntGun.speed;
            pathHandler.isReached = false;
            direction = GameManager.Instance.player.transform.position - controller.transform.position;
            Debug.Log("Jaunt Triggered");
        }

        public override void EnterState()
        {
            pathHandler.SetTarget(jauntGun.target);
            ShootNGun();
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

        private IEnumerator ShootNGun()
        {
            WaitForSeconds fireRate = new WaitForSeconds(jauntGun.fireRate);

            Debug.Log($"PathHandler is {pathHandler}");
            while(pathHandler.isReached != false)
            {
                GameObject bullet = Controller.Instantiate(jauntGun.bullet, Controller.transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = jauntGun.speed * direction.normalized;    
                yield return fireRate;
            }
            Controller.TriggeredOnMoveComplete();
        }
    }
}