using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "RunNGun", menuName = "NPC/RunNGun")]
    public class RunNGun : Move
    {
        public GameObject bullet;
        public float speed;
        public int range;
        public float bulletSpeed;
        public float fireRate;
        public float delay;
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