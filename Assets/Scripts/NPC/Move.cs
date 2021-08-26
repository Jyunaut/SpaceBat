using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : ScriptableObject
{
    public float duration;
    public bool looping;
    public int uses { get; set; } // TODO add condition so that uses can be initialized if NOT looping
}
