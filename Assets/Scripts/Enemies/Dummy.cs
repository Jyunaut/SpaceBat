using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Actor
{
    private void OnEnable()
    {
        GlobalEvents.OnReachedEndOfLevel += Test;
    }

    private void OnDisable()
    {
        GlobalEvents.OnReachedEndOfLevel -= Test;
    }

    private void Test()
    {
        Debug.Log("Woohoo");
    }
}
