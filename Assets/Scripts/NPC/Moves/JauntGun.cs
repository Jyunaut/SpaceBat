using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "JauntGun", menuName = "NPC/JauntGun")]
    public class JauntGun : Move
    {
        public GameObject bullet;
        public Vector3 target;
        public float speed;
        public float bulletSpeed;
        public float fireRate;
        public float delay;
        public float stopDistance;
    }
}