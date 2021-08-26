using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Phase", menuName = "NPC Phase")]
    public class Phase : ScriptableObject
    {
        public List<Move> moves;
        public bool looping;
    }
}

