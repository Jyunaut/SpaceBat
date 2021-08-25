using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "Phase", menuName = "NPC/Phase")]
    public class Phase : ScriptableObject
    {
        public Move[] moves;
    }
}

