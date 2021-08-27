using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Shotgun", menuName = "NPC/Shotgun")]
    public class Shotgun : Move
    {
        [System.Serializable]
        public struct Path
        {
            public GameObject bullet;
            public Vector2 direction;
            public float speed;
        } public Path[] paths;
        public int totalShots = 1;
        public float fireRate;
        public float delay;
    }
}