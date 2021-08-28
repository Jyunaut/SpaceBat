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
            pathHandler.SetTarget(jauntGun.target);
        }

        public override void EnterState()
        {
            Controller.StartCoroutine(ShootNGun());
        }

        public override void FixedUpdate()
        {
            Debug.Log(pathHandler.isReached);
            if (!Controller.IsStaggered)
            {
                pathHandler.HandleMovement();
            }
        }

        public override void Transitions()
        {
            if (pathHandler.isReached)
            {
                pathHandler.StopMoving();
                Controller.TriggeredOnMoveComplete();
            }
            // else if (moveToPlayer.animation != null && timer >= moveToPlayer.animation.clip.length)
            //     Controller.TriggeredOnMoveComplete();
            // timer += Time.deltaTime;
        }

        private IEnumerator ShootNGun()
        {
            yield return new WaitForSeconds(jauntGun.delay);

            WaitForSeconds fireRate = new WaitForSeconds(jauntGun.fireRate);
            while(!pathHandler.isReached)
            {
                GameObject bullet = Controller.Instantiate(jauntGun.bullet, Controller.transform.position, Quaternion.identity);
                direction = GameManager.Instance.player.transform.position - Controller.transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = jauntGun.bulletSpeed * direction.normalized;    
                yield return fireRate;
            }
            Controller.TriggeredOnMoveComplete();
        }
    }
}