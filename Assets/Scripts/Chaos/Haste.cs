using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [CreateAssetMenu(fileName = "Haste", menuName = "Chaos Effect/Haste")]
    public class Haste : ChaosEffect
    {
        public float speedIncrease = 1f;
        
        public override void DoEffect()
        {
            Debug.Log($"Everyone go faster by x{speedIncrease}");
        }
    }
}