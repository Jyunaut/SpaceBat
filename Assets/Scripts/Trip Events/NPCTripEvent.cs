using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPCTripEvent : TripEvent
{
    [System.Serializable]
    private struct NPC
    {
        [SerializeField] public GameObject npc;
        [SerializeField] public Vector2 position;
        [SerializeField] public float spawnDelay;
        [SerializeField] public bool isMobile;
    } [SerializeField] private List<NPC> _NPCToSpawn;

    protected override void DoEvent()
    {
        for (int i = 0; i < _NPCToSpawn.Count; i++)
        {
            StartCoroutine(SpawnAfterDelay(_NPCToSpawn[i].npc, _NPCToSpawn[i].position, _NPCToSpawn[i].spawnDelay, _NPCToSpawn[i].isMobile));
        }
    }

    private IEnumerator SpawnAfterDelay(GameObject npc, Vector2 position, float delay, bool isMobile)
    {
        Vector2 initPos = transform.position;
        yield return new WaitForSeconds(delay);
        npc.SetActive(true);
        npc.transform.position = isMobile ? initPos + position : (Vector2)transform.position + position;
        if (isMobile) npc.transform.SetParent(null);
    }

    private const string _kSpawnIconName = "redsquare.png";
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _NPCToSpawn.Count; i++)
        {
            Gizmos.DrawIcon((Vector2)transform.position + _NPCToSpawn[i].position, _kSpawnIconName, true);
        }
    }
}