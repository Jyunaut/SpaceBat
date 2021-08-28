using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Despawn", menuName = "NPC/Despawn")]
    public class Despawn : Move
    {
        public GameObject despawnEffect;
    }
}