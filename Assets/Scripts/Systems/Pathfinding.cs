using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public static Pathfinding Instance { get; private set; }

    public Grid<PathNode> grid;
    public Vector3 origin;
    public int width;
    public int height;
    public float cellSize;
    public const int DIAGONAL_COST = 14;
    public const int STRAIGHT_COST = 10;
    public List<PathNode> openList;
    public List<PathNode> closedList;

    public List<PathNode> debugTraversedNodes;
    public bool DebugNodeType = true;
    public string a = "lmao";

    public Pathfinding(int width, int height, Vector3 origin, float cellSize)
    {
        Instance = this;
        grid = new Grid<PathNode>(width, height, cellSize, origin, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y, cellSize));
        this.width = width;
        this.height = height;
        this.origin = origin;
        this.cellSize = cellSize;

        if (DebugNodeType)
            debugTraversedNodes = new List<PathNode>();
    }

    public void DebugPathNodeType()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid.GetGridObject(x, y).DebugOnUpdate();
            }
        }
    }

    public bool DebugLogNode(Vector3 target)
    {
        return (grid.GetGridObject(target) != null) ? true : false;
    }

    public PathNode GetPathNode(Vector3 worldPosition)
    {
        return grid.GetGridObject(worldPosition);
    }

    public List<Vector3> FindPath(Vector3 origin, Vector3 target)
    {
        grid.GetXY(origin, out int startX, out int startY);
        grid.GetXY(target, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);

        if (path == null) return null;

        List<Vector3> vectorPath = new List<Vector3>();
        foreach (PathNode n in path)
        {
            Vector3 center = n.GetWorldPosition() + (new Vector3(cellSize, cellSize) * 0.5f);
            vectorPath.Add(center);
        }


        return vectorPath;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestF();
            if (currentNode == endNode)
            {
                // Path found
                Debug.Log("Path Found");
                return CalculatePath(endNode, startNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode adjacentNode in GetAdjacentNodes(currentNode))
            {
                if (closedList.Contains(adjacentNode))
                    continue;

                if (adjacentNode.gScore < currentNode.gScore || !openList.Contains(adjacentNode))
                {
                    ScoreNode(adjacentNode, startNode, endNode);
                    adjacentNode.parent = currentNode;

                    if (!openList.Contains(adjacentNode))
                    {
                        openList.Add(adjacentNode);
                    }
                }
            }
        }

        // No path found
        Debug.Log("There is no path");
        return null;
    }

    private List<PathNode> CalculatePath(PathNode endNode, PathNode startNode)
    {
        List<PathNode> path = new List<PathNode>();

        PathNode currentNode = endNode;
        PathNode startingNode = startNode;
        while (currentNode != startingNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startingNode);
        path.Reverse();

        //SET DEBUG NODE TYPES
        if (DebugNodeType)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    grid.GetGridObject(x, y).NodeType = PathNode.DebugNodeType.NotSet;

            foreach (PathNode o in openList)
                o.NodeType = PathNode.DebugNodeType.Openlist;

            foreach (PathNode c in closedList)
                c.NodeType = PathNode.DebugNodeType.Closedlist;

            foreach (PathNode p in path)
                p.NodeType = PathNode.DebugNodeType.Path;
        }

        return path;
    }

    private List<PathNode> GetAdjacentNodes(PathNode currentNode)
    {
        List<PathNode> adjacentNodes = new List<PathNode>();

        for (int x = currentNode.x - 1; x != currentNode.x + 2; x++)
        {
            for (int y = currentNode.y - 1; y != currentNode.y + 2; y++)
            {
                PathNode adjacentNode = grid.GetGridObject(x, y);
                if (adjacentNode != null && adjacentNode.IsWalkable())
                {
                    adjacentNodes.Add(adjacentNode);

                    if (!debugTraversedNodes.Contains(adjacentNode))
                        debugTraversedNodes.Add(adjacentNode);
                }
            }
        }
        adjacentNodes.Remove(currentNode);

        return adjacentNodes;
    }

    private void ScoreNode(PathNode node, PathNode origin, PathNode target)
    {
        node.gScore = CalculateDistanceCost(node, origin);
        node.hScore = CalculateDistanceCost(node, target);
        node.fScore = node.gScore + node.hScore;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestF()
    {
        PathNode lowestFScore = openList[0];
        for (int i = 1; i < openList.Count; i++)
        {
            if (openList[i].fScore < lowestFScore.fScore)
            {
                lowestFScore = openList[i];
            }
        }
        return lowestFScore;
    }
}

public class PathNode
{
    public enum DebugNodeType
    {
        NotSet,
        Openlist,
        Closedlist,
        Path,
        ColliderCheck
    }
    public DebugNodeType NodeType { get; set; }
    public int gScore { get; set; }
    public int hScore { get; set; }
    public int fScore { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public float cellSize { get; set; }
    public PathNode parent { get; set; }

    private Grid<PathNode> _grid;

    public PathNode(Grid<PathNode> grid, int x, int y, float cellSize)
    {
        _grid = grid;
        this.x = x;
        this.y = y;
        _grid.SetGridObject(x, y, this);
        this.cellSize = cellSize;

        NodeType = DebugNodeType.NotSet;
    }

    public void DebugOnUpdate()
    {
        bool debugEnabled = true;

        if (debugEnabled)
        {
            float offset = 0.2f;
            Vector3 diagonal1Start = GetWorldPosition() + new Vector3(offset, offset);
            Vector3 diagonal1End = GetWorldPosition() + new Vector3(cellSize - offset, cellSize - offset);

            Vector3 diagonal2Start = GetWorldPosition() + new Vector3(cellSize - offset, offset);
            Vector3 diagonal2End = GetWorldPosition() + new Vector3(offset, cellSize - offset);

            switch (NodeType)
            {
                case DebugNodeType.NotSet:
                    break;
                case DebugNodeType.Openlist:
                    Debug.DrawLine(diagonal1Start, diagonal1End, Color.blue);
                    Debug.DrawLine(diagonal2Start, diagonal2End, Color.blue);
                    break;
                case DebugNodeType.Closedlist:
                    Debug.DrawLine(diagonal1Start, diagonal1End, Color.red);
                    Debug.DrawLine(diagonal2Start, diagonal2End, Color.red);
                    break;
                case DebugNodeType.Path:
                    Debug.DrawLine(diagonal1Start, diagonal1End, Color.green);
                    Debug.DrawLine(diagonal2Start, diagonal2End, Color.green);
                    break;
                default:
                    break;
            }
        }
    }

    public bool IsWalkable()
    {
        float offset = 0.2f;
        Vector3 centerPosition = GetWorldPosition() + new Vector3(cellSize, cellSize) * .5f;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(centerPosition, new Vector2(cellSize - offset, cellSize - offset), 0, LayerMask.GetMask(GlobalStrings.kObstacle));
        if (colliders.Length > 0)
        Debug.Log(colliders[0].gameObject.layer);

        if (colliders.Length > 0)
        {
            return false;
        }
        return true;
    }

    public bool IsGrounded()
    {
        if (!_grid.GetGridObject(x, y - 1).IsWalkable())
        {
            return true;
        }
        return false;
    }

    public bool IsWalled()
    {
        if (!_grid.GetGridObject(x - 1, y).IsWalkable() || !_grid.GetGridObject(x + 1, y).IsWalkable())
        {
            return true;
        }
        return false;
    }

    public Vector3 GetWorldPosition()
    {
        return _grid.GetWorldPosition(x, y);
    }

    public void CalculateFScore()
    {
        fScore = gScore + hScore;
    }

    public override string ToString()
    {
        string position = x + ", " + y;
        string score = gScore + ", " + hScore + ", " + fScore;
        string parent = "";
        if (this.parent != null)
            parent = this.parent.x + ", " + this.parent.y;
        return position + "\n>S" + score + "\n>P" + parent;
    }
}
