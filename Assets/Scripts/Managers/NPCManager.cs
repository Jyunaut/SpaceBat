using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

    public float cellSize;
    public Rect levelRect;
    public GameObject player;
    public Pathfinding pathfinding;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        pathfinding = new Pathfinding((int)levelRect.width, (int)levelRect.height, levelRect.position, cellSize);
    }

    private void Update()
    {
        pathfinding.DebugPathNodeType();
    }

    public int a(int b = 0)
    {
        return b;
    }
}
