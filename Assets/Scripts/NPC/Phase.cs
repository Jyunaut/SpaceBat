using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Phase", menuName = "NPC/Phase")]
    public class Phase : ScriptableObject
    {
        public Move[] moves;
    }
}

