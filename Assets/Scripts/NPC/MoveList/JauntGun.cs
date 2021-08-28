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
        private Coroutine coroutine;
        
        private const float checkInterval = 0.25f;
        private float timer;

        public JauntGun(Controller controller) : base(controller)
        {
            jauntGun = (MoveLibrary.JauntGun)Controller.CurrentMove;
            pathHandler = Controller.GetComponent<PathfindingHandler>();
            pathHandler.speed = jauntGun.speed;
            coroutine = Controller.StartCoroutine(ShootNGun());
        }

        public override void FixedUpdate()
        {
            if (!Controller.IsStaggered)
            {
                pathHandler.SetTarget(jauntGun.target);
                pathHandler.HandleMovement();
            }
        }

        public override void Transitions()
        {
            if (timer >= checkInterval)
            {
                if ((jauntGun.target - Controller.transform.position).magnitude <= jauntGun.stopDistance)
                {
                    pathHandler.StopMoving();
                    Controller.StopCoroutine(coroutine);
                    Controller.TriggeredOnMoveComplete();
                }
                timer = 0f;
            }
            timer += Time.deltaTime;
        }

        private IEnumerator ShootNGun()
        {
            yield return new WaitForSeconds(jauntGun.startDelay);

            WaitForSeconds fireRate = new WaitForSeconds(jauntGun.fireRate);

            while(true)
            {
                GameObject bullet = Controller.Instantiate(jauntGun.bullet, Controller.transform.position, Quaternion.identity);
                direction = GameManager.Instance.player.transform.position - Controller.transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = jauntGun.bulletSpeed * direction.normalized;
                yield return fireRate;
            }
        }
    }
}