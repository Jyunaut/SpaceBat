using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Explode", menuName = "NPC/Explode")]
    public class Explode : Move
    {
        public GameObject explosion;
        public int damage;
        public float radius;
        public bool deathOnExplode;
    }
}