using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class RunNGun : State
    {
        private MoveLibrary.RunNGun runNGun;
        private PathfindingHandler pathHandler;
        private Vector3 direction;
        private float timer = 0;

        public RunNGun(Controller controller) : base(controller)
        {
            runNGun = (MoveLibrary.RunNGun)Controller.CurrentMove;
            pathHandler = Controller.GetComponent<PathfindingHandler>();
            pathHandler.speed = runNGun.speed;
            runNGun.direction = runNGun.direction.normalized;
            pathHandler.SetTarget(GetTarget());
            Debug.Log("Runner Triggered");
        }

        public override void EnterState()
        {
            // Controller.StartCoroutine(ShootNGun());
        }

        public override void Update()
        {
            Debug.Log(pathHandler.isReached);
            if(!Controller.IsStaggered)
            {
                pathHandler.HandleMovement();
            }
            else
            {
                pathHandler.StopMoving();
            }
        }

        public override void Transitions()
        {
            if (pathHandler.isReached)
            {
                pathHandler.SetTarget(GetTarget());
                // Controller.TriggeredOnMoveComplete();
            }
            // else if (moveToPlayer.animation != null && timer >= moveToPlayer.animation.clip.length)
            //     Controller.TriggeredOnMoveComplete();
            // timer += Time.deltaTime;
        }

        private Vector3 GetTarget()
        {
            Pathfinding.Instance.grid.GetXY(Controller.transform.position, out int x, out int y);
            x = Mathf.Abs(x) + Mathf.Abs(runNGun.range) * (int)runNGun.direction.x;
            y = Mathf.Abs(y) + Mathf.Abs(runNGun.range) * (int)runNGun.direction.y;

            return Pathfinding.Instance.grid.GetWorldPosition(x, y); ;
        }

        private IEnumerator ShootNGun()
        {
            yield return new WaitForSeconds(runNGun.delay);

            WaitForSeconds fireRate = new WaitForSeconds(runNGun.fireRate);
            while(pathHandler.isReached == false)
            {
                GameObject bullet = Controller.Instantiate(runNGun.bullet, Controller.transform.position, Quaternion.identity);
                direction = GameManager.Instance.player.transform.position - Controller.transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = runNGun.bulletSpeed * direction.normalized;    
                yield return fireRate;
            }
            Controller.TriggeredOnMoveComplete();
        }
    }
}