using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "MoveToTarget", menuName = "NPC/MoveToTarget")]
    public class MoveToTarget : Move
    {
        public Animation animation;
        public float speed;
    }
}