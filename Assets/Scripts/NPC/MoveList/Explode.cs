using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Explode : State
    {
        private MoveLibrary.Explode explode;
        private Coroutine attack;

        public Explode(Controller controller) : base(controller)
        {
            explode = (MoveLibrary.Explode)Controller.CurrentMove;
        }

        public override void EnterState()
        {
            if (Controller.Animator.runtimeAnimatorController != null)
                Controller.Animator.Play("Charge");
            attack = Controller.StartCoroutine(DelayedExplosion());
        }

        public override void ExitState()
        {
            Controller.StopCoroutine(attack);
        }

        private IEnumerator DelayedExplosion()
        {
            yield return new WaitForSeconds(explode.startDelay);
            Collider2D hit = Physics2D.OverlapCircle(Controller.transform.position, explode.radius, LayerMask.GetMask(GlobalStrings.kPlayer));
            if (hit != null && hit.TryGetComponent(out Actor target))
            {
                target.TakeDamage(explode.damage);
            }
            if (explode.explosion)
                Controller.Instantiate(explode.explosion, Controller.transform.position, Quaternion.identity);
            else
                Debug.LogWarning("Missing Explosion Prefab.", explode);
            
            if (explode.deathOnExplode)
                Controller.Destroy(Controller.gameObject);
            else
            {
                yield return new WaitForSeconds(explode.endDelay);
                Controller.TriggeredOnMoveComplete();
            }
        }
    }
}