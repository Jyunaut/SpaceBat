using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingHandler : MonoBehaviour
{
    public int currentPathIndex;
    public float speed;
    public bool isReached { get; set; } = false;
    public List<Vector3> vectorPath;

    private void Start()
    {
        StopMoving();
        currentPathIndex = 0;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (vectorPath != null)
        {
            Vector3 targetPosition = vectorPath[currentPathIndex];
            if (Vector3.Distance(GetPosition(), targetPosition) > 0.5f)
            {
                Vector3 moveDir = (targetPosition - GetPosition()).normalized;
                transform.position += moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if(currentPathIndex >= vectorPath.Count)
                {
                    StopMoving();
                    isReached = true;
                }
            }
        }
    }

    public void SetTarget(Vector3 target)
    {
        currentPathIndex = 0;
        vectorPath = Pathfinding.Instance.FindPath(GetPosition(), target);

        if (vectorPath != null && vectorPath.Count > 1)
        {
            vectorPath.RemoveAt(0);
        }
    }

    private void StopMoving()
    {
        vectorPath = null;
    }

    private Vector3 GetPosition()
    {
        return transform.position;
    }

}
