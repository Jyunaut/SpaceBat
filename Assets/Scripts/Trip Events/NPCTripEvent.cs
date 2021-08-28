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
        [SerializeField] public float spawnDelay;
        [SerializeField] public bool isMobile;
    } [SerializeField] private List<NPC> _NPCToSpawn;

    protected override void DoEvent()
    {
        for (int i = 0; i < _NPCToSpawn.Count; i++)
        {
            if (_NPCToSpawn[i].npc == null)
                Debug.LogWarning("Missing Enemy Prefab.", this);
            else
                StartCoroutine(SpawnAfterDelay(_NPCToSpawn[i].npc, _NPCToSpawn[i].npc.transform.position, _NPCToSpawn[i].spawnDelay, _NPCToSpawn[i].isMobile));
        }
    }

    private IEnumerator SpawnAfterDelay(GameObject npc, Vector2 position, float delay, bool isMobile)
    {
        Vector2 initPos = transform.position;
        yield return new WaitForSeconds(delay);
        npc.SetActive(true);
        // npc.transform.position = isMobile ? initPos + position : (Vector2)transform.position + position;
        if (isMobile) npc.transform.SetParent(null);
    }

    private const string _kSpawnIconName = "redsquare.png";
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _NPCToSpawn.Count; i++)
        {
            if (_NPCToSpawn[i].npc)
                Gizmos.DrawIcon((Vector2)(transform.position + _NPCToSpawn[i].npc.transform.position), _kSpawnIconName, true);
        }
    }

    private void OnValidate()
    {
        for (int i = 0; i < _NPCToSpawn.Count; i++)
        {
            if (_NPCToSpawn[i].npc && _NPCToSpawn[i].npc.transform.parent != transform)
            {
                _NPCToSpawn[i].npc.transform.parent = transform;
                _NPCToSpawn[i].npc.SetActive(false);
            }
        }
    }
}