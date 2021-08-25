using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTripEvent : TripEvent
{
    private GameObject _boss;

    private void Awake()
    {
        _boss = GameObject.FindGameObjectWithTag(GlobalStrings.kBoss);
    }

    protected override void DoEvent()
    {
        GlobalEvents.EndOfLevelReached();
        StartCoroutine(BossIntro());
    }

    private IEnumerator BossIntro()
    {
        // Disable Boss AI: _boss.DisableAI();
        _boss.GetComponent<Animator>().Play("Intro");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(_boss.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        // Enable Boss AI: _boss.EnableAI();
    }
}