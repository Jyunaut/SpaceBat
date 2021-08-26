using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "ShootDirectional", menuName = "NPC/ShootDirectional")]
    public class ShootDirectional : Move
    {
        public GameObject[] helperObj;
        public int damage;
        public float radius;
        public float delay;
    }
}