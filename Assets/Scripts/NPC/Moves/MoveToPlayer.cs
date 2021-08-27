using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "MoveToPlayer", menuName = "NPC/MoveToPlayer")]
    public class MoveToPlayer : Move
    {
        public float speed;
        public float stopDistance;
        public bool followForever;
    }
}