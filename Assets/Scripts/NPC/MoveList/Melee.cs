using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Melee : State
    {
        private MoveLibrary.Melee melee;
        
        public Melee(Controller controller) : base(controller)
        {
            melee = (MoveLibrary.Melee)Controller.CurrentMove;
        }

        public override void EnterState()
        {
            Controller.StartCoroutine(AttackAfterDelay());
        }

        private IEnumerator AttackAfterDelay()
        {
            yield return new WaitForSeconds(melee.delay);

            Vector2 origin = (Vector2)Controller.transform.position + melee.hitbox.origin;
            Vector2 size = melee.hitbox.size;
            Collider2D hit = Physics2D.OverlapBox(origin, size, 0f, LayerMask.GetMask(GlobalStrings.kPlayer));
            if (hit && hit.TryGetComponent(out Actor target))
            {
                target.TakeDamage(melee.damage);
            }
            Vector2[] points =
            {
                new Vector2(origin.x - size.x / 2f, origin.y + size.y / 2f),
                new Vector2(origin.x + size.x / 2f, origin.y + size.y / 2f),
                new Vector2(origin.x - size.x / 2f, origin.y - size.y / 2f),
                new Vector2(origin.x + size.x / 2f, origin.y - size.y / 2f)
            };
            Debug.DrawLine(points[0], points[1], Color.red, 1f); // top
            Debug.DrawLine(points[2], points[3], Color.red, 1f); // bottom
            Debug.DrawLine(points[0], points[2], Color.red, 1f); // left
            Debug.DrawLine(points[1], points[3], Color.red, 1f); // right
            Controller.TriggeredOnMoveComplete();
        }
    }
}