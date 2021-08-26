using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [CreateAssetMenu(fileName = "Power", menuName = "Chaos Effect/Power")]
    public class Power : ChaosEffect
    {
        public float damageIncrease = 1f;
        
        public override void DoEffect()
        {
            Debug.Log($"Everyone do x{damageIncrease} more damage");
        }
    }
}