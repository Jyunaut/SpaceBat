using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Shotgun : State
    {
        private MoveLibrary.Shotgun shotgun;

        public Shotgun(Controller controller) : base(controller)
        {
            shotgun = (MoveLibrary.Shotgun)Controller.currentMove;
        }

        public override void EnterState()
        {
            Controller.StartCoroutine(ShootAfterDelay());
        }

        private IEnumerator ShootAfterDelay()
        {
            yield return new WaitForSeconds(shotgun.delay);

            WaitForSeconds fireRate = new WaitForSeconds(shotgun.fireRate);
            for (int i = 0; i < shotgun.totalShots; i++)
            {
                for (int j = 0; j < shotgun.paths.Length; j++)
                {
                    GameObject bullet = Controller.Instantiate(shotgun.paths[j].bullet, Controller.transform.position, Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().velocity = shotgun.paths[j].speed * shotgun.paths[j].direction;
                }
                yield return fireRate;
            }
            Controller.TriggeredOnMoveComplete();
        }
    }
}