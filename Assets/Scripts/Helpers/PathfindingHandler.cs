using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingHandler : MonoBehaviour
{
    public int currentPathIndex;
    public float speed;
    public bool isReached;
    public List<Vector3> vectorPath;
    private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StopMoving();
        currentPathIndex = 0;
    }

    public void HandleMovement()
    {
        if (vectorPath != null)
        {
            Vector3 targetPosition = vectorPath[currentPathIndex];
            if (Vector3.Distance(GetPosition(), targetPosition) > 0.1f)
            {
                Vector3 moveDir = (targetPosition - GetPosition()).normalized;
                rigidbody2d.MovePosition(GetPosition() + moveDir * speed * Time.deltaTime);
            }
            else
            {
                currentPathIndex++;
                if(currentPathIndex >= vectorPath.Count)
                {
                    StopMoving();
                }
            }
        }
    }

    public void SetTarget(Vector3 target)
    {
        isReached = false;
        currentPathIndex = 0;
        vectorPath = Pathfinding.Instance.FindPath(GetPosition(), target);

        if (vectorPath != null && vectorPath.Count > 1)
        {
            vectorPath.RemoveAt(0);
        }
    }

    public void StopMoving()
    {
        rigidbody2d.velocity = Vector2.zero;
        isReached = true;
        vectorPath = null;
    }

    private Vector3 GetPosition()
    {
        return transform.position;
    }

}
