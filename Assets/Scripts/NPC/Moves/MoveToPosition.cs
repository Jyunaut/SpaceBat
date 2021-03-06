using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "MoveToPosition", menuName = "NPC/MoveToPosition")]
    public class MoveToPosition : Move
    {
        public Vector2 position;
        [Min(0.1f)] public float travelTime;
    }
}