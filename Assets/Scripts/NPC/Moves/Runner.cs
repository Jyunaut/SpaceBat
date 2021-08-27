using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Runner", menuName = "NPC/Runner")]
    public class Runner : Move
    {
        public float speed;
        public float range;
    }
}