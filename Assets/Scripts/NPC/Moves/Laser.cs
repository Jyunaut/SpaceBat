using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Laser", menuName = "NPC/Laser")]
    public class Laser : Move
    {
        public GameObject laser;
        public GameObject telegraphEffect;
        public int damage;
        [System.Serializable]
        public struct Hitbox
        {
            public Vector2 origin;
            public Vector2 size;
            [Range(0f, 360f)] public float angle;
        } public Hitbox hitbox;
    }
}