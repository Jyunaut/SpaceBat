using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTripEvent : TripEvent
{
    protected override void DoEvent()
    {
        SceneController.LoadScene(0);
    }
}