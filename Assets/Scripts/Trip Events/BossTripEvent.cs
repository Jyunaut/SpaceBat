using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTripEvent : TripEvent
{
    private GameObject _boss;

    private void Awake()
    {
        try
        {
            _boss = GameObject.FindGameObjectWithTag(GlobalStrings.kBoss);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e, this);
        }
    }

    protected override void DoEvent()
    {
        GlobalEvents.EndOfLevelReached();
        if (_boss == null)
            GlobalEvents.CompleteLevel();
        else
            StartCoroutine(BossIntro());
    }

    private IEnumerator BossIntro()
    {
        // Disable Boss AI: _boss.DisableAI();
        // _boss.GetComponent<Animator>().Play("Intro");
        yield return new WaitForEndOfFrame();
        // yield return new WaitForSeconds(_boss.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        // Enable Boss AI: _boss.EnableAI();
    }
}