using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : ScriptableObject
{
    public GameObject[] helperObj;
    public float duration;
    public bool looping = true;
    public float startDelay;
    public float endDelay;
}
