using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Jaunt", menuName = "NPC/Jaunt")]
    public class Jaunt : Move
    {
        public Vector3 target;
        public float speed;
    }
}

