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
            yield return new WaitForSeconds(jauntGun.delay);

            WaitForSeconds fireRate = new WaitForSeconds(jauntGun.fireRate);
            while(pathHandler.isReached == false)
            {
                GameObject bullet = Controller.Instantiate(jauntGun.bullet, Controller.transform.position, Quaternion.identity);
                direction = GameManager.Instance.player.transform.position - Controller.transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = jauntGun.speed * direction.normalized;    
                yield return fireRate;
            }
            Controller.TriggeredOnMoveComplete();
        }
    }
}