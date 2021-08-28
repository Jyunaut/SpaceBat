using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Shotgun : State
    {
        private MoveLibrary.Shotgun shotgun;
        private Coroutine attack;

        public Shotgun(Controller controller) : base(controller)
        {
            shotgun = (MoveLibrary.Shotgun)Controller.CurrentMove;
        }

        public override void EnterState()
        {
            attack = Controller.StartCoroutine(ShootAfterDelay());
        }

        public override void ExitState()
        {
            Controller.StopCoroutine(attack);
        }

        private IEnumerator ShootAfterDelay()
        {
            yield return new WaitForSeconds(shotgun.startDelay);

            WaitForSeconds fireRate = new WaitForSeconds(shotgun.fireRate);
            for (int i = 0; i < shotgun.totalShots; i++)
            {
                for (int j = 0; j < shotgun.paths.Length; j++)
                {
                    GameObject bullet = Controller.Instantiate(shotgun.paths[j].bullet, Controller.transform.position, Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().velocity = shotgun.paths[j].speed * shotgun.paths[j].direction.normalized;
                }
                yield return fireRate;
            }
            EffectsManager.Instance.ScreenShake(shotgun.screenShake);
            yield return new WaitForSeconds(shotgun.endDelay);
            Controller.TriggeredOnMoveComplete();
        }
    }
}