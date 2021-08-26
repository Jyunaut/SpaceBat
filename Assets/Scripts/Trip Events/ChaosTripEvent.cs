using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosTripEvent : TripEvent
{
    [SerializeField] private List<ChaosEffect> _chaosEffect;

    protected override void DoEvent()
    {
        if (_chaosEffect.Count > 0)
            foreach (ChaosEffect effect in _chaosEffect)
                effect.DoEffect();
    }
}
