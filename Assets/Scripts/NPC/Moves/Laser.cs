using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Laser", menuName = "NPC/Laser")]
    public class Laser : Move
    {
        public GameObject laser;
        public int damage;
        [System.Serializable]
        public struct Hitbox
        {
            public Vector2 origin;
            public Vector2 size;
        } public Hitbox hitbox;
        public float delay;
    }
}