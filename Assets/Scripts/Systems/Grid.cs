using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid<TObject>
{
    private int _x;
    private int _y;
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _origin;
    private TObject[,] grid;

    public Grid(int width, int height, float cellSize, Vector3 origin, Func<Grid<TObject>, int, int, TObject> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _origin = origin;

        grid = new TObject[_width, _height];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = createGridObject(this, x, y);
            }
        }

        // Debug
        bool DebugLines = false;
        if(DebugLines)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.black, 100f);
            Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.black, 100f);
        }
    }

    public void SetGridObject(int x, int y, TObject obj)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            grid[x, y] = obj;
        }
    }

    public void SetGridObject(Vector3 worldPosition, TObject obj)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, obj);
    }

    public TObject GetGridObject(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < _width && y < _height) ? grid[x, y] : default(TObject);
    }

    public TObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _origin;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _origin).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _origin).y / _cellSize);
    }
}
