using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Explode : State
    {
        private MoveLibrary.Explode explode;

        public Explode(Controller controller) : base(controller)
        {
            explode = (MoveLibrary.Explode)Controller.currentMove;
        }

        public override void EnterState()
        {
            if (Controller.Animator.runtimeAnimatorController != null)
                Controller.Animator.Play("Charge");
            Controller.StartCoroutine(DelayedExplosion());
        }

        private IEnumerator DelayedExplosion()
        {
            yield return new WaitForSeconds(explode.delay);
            Collider2D hit = Physics2D.OverlapCircle(Controller.transform.position, explode.radius, LayerMask.GetMask(GlobalStrings.kPlayer));
            if (hit != null && hit.TryGetComponent(out Actor target))
            {
                target.TakeDamage(explode.damage);
            }
            Controller.Instantiate(explode.explosion, Controller.transform.position, Quaternion.identity);
            Controller.Destroy(Controller.gameObject);
        }
    }
}