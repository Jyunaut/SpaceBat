using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Melee : State
    {
        private MoveLibrary.Melee melee;
        private Coroutine attack;
        
        public Melee(Controller controller) : base(controller)
        {
            melee = (MoveLibrary.Melee)Controller.CurrentMove;
        }

        public override void EnterState()
        {
            attack = Controller.StartCoroutine(AttackAfterDelay());
        }

        public override void ExitState()
        {
            Controller.StopCoroutine(attack);
        }

        private IEnumerator AttackAfterDelay()
        {
            yield return new WaitForSeconds(melee.startDelay);

            Vector2 origin = (Vector2)Controller.transform.position + melee.hitbox.origin;
            Vector2 size = melee.hitbox.size;
            Collider2D hit = Physics2D.OverlapBox(origin, size, 0f, LayerMask.GetMask(GlobalStrings.kPlayer));
            if (hit && hit.TryGetComponent(out Actor target))
            {
                target.TakeDamage(melee.damage);
                EffectsManager.Instance.TimeSlow(melee.hitStop);
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
            EffectsManager.Instance.ScreenShake(melee.screenShake);
            yield return new WaitForSeconds(melee.endDelay);
            Controller.TriggeredOnMoveComplete();
        }
    }
}