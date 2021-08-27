using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Laser : State
    {
        private MoveLibrary.Laser laser;
        private Coroutine attack;

        public Laser(Controller controller) : base(controller)
        {
            laser = (MoveLibrary.Laser)Controller.CurrentMove;
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
            yield return new WaitForSeconds(laser.delay);
            Vector2 origin = (Vector2)Controller.transform.position + laser.hitbox.origin;
            Vector2 size = laser.hitbox.size;
            Collider2D hit = Physics2D.OverlapBox(origin, size, 0f, LayerMask.GetMask(GlobalStrings.kPlayer));
            if (hit && hit.TryGetComponent(out Actor target))
            {
                target.TakeDamage(laser.damage);
            }
            Vector2[] points =
            {
                new Vector2(origin.x - size.x / 2f, origin.y + size.y / 2f),
                new Vector2(origin.x + size.x / 2f, origin.y + size.y / 2f),
                new Vector2(origin.x - size.x / 2f, origin.y - size.y / 2f),
                new Vector2(origin.x + size.x / 2f, origin.y - size.y / 2f)
            };
            Debug.DrawLine(points[0], points[1], Color.red, 0.5f); // top
            Debug.DrawLine(points[2], points[3], Color.red, 0.5f); // bottom
            Debug.DrawLine(points[0], points[2], Color.red, 0.5f); // left
            Debug.DrawLine(points[1], points[3], Color.red, 0.5f); // right
            Controller.TriggeredOnMoveComplete();
        }
    }
}