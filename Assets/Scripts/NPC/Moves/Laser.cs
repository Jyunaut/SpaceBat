using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Laser", menuName = "NPC/Laser")]
    public class Laser : Move
    {
        public GameObject[] helperObj;
        public int damage;
        public float radius;
        public float delay;
    }
}