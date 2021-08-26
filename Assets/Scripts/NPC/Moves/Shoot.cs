using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveLibrary
{
    [CreateAssetMenu(fileName = "Shoot", menuName = "NPC/Shoot")]
    public class Shoot : Move
    {
        public GameObject bullet;
    }
}
