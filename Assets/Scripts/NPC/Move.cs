using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : ScriptableObject
{
    public GameObject[] helperObj;
    public float duration;
    public bool looping = true;
    public bool background;
    public int uses
    {
        get { return uses; }
        set
        {
            if (looping == false)
                uses = value;
            else
                Debug.Log($"Cannot assign uses while looping is {looping}");
        }
    }
}
