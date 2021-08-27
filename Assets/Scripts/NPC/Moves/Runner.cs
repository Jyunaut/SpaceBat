using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Runner", menuName = "NPC/Runner")]
    public class Runner : Move
    {
        public float speed;
        public int range;
        public Vector3 direction
        {
            get
            {
                return dir;
            }
            set 
            {
                dir = value;
                dir = dir.normalized;
            }
        }
        [SerializeField] private Vector3 dir;
    }
}