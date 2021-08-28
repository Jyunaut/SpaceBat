using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    class Despawn : State
    {
        private MoveLibrary.Despawn despawn;

        public Despawn(Controller controller) : base(controller)
        {
            despawn = (MoveLibrary.Despawn)Controller.CurrentMove;
        }

        public override void EnterState()
        {
            Controller.StartCoroutine(DespawnAfterDelay());
        }

        private IEnumerator DespawnAfterDelay()
        {
            yield return new WaitForSeconds(despawn.startDelay);
            if (despawn.despawnEffect)
                Controller.Instantiate(despawn.despawnEffect, Controller.transform.position, Quaternion.identity);
            Controller.Destroy(Controller.gameObject);
        }
    }
}