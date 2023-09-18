using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemHex<TGridObject> 
{
    private int height;
    private int width;
    private float cellSize;

    private TGridObject[,] gridObjectsArr;
    public int Width 
    {
        get{return width;}
    }
    public int Height 
    {
        get{return height;}
    }

    private const float VERTICAL_OFFSET_MULTIPLIER = 0.75f;

    public GridSystemHex(int width, int height, float cellSize, Func<GridSystemHex<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridObjectsArr = new TGridObject[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z ++)
            {
                GridPosition gridpos = new GridPosition(x, z);
                gridObjectsArr[x, z] = createGridObject(this, gridpos);
                // Debug.DrawLine(GetWorldPosition(gridpos), GetWorldPosition(gridpos) + Vector3.right * 0.2f, Color.white, 1000);
            }
        }
       
    }
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0 ,0) * cellSize +
               new Vector3(0, 0, gridPosition.z) * cellSize * VERTICAL_OFFSET_MULTIPLIER +
                   (((gridPosition.z %2) == 1) ? new Vector3(1, 0, 0) * cellSize/2 : Vector3.zero);
    }
    public GridPosition GetGridPosition(Vector3 worldP0sition)
    {
        GridPosition roughXZ = new GridPosition(Mathf.RoundToInt(worldP0sition.x /cellSize), Mathf.RoundToInt(worldP0sition.z/cellSize/VERTICAL_OFFSET_MULTIPLIER));
        bool oddRow = roughXZ.z % 2 == 1;
        List<GridPosition> neighbourGridPositions = new List<GridPosition>()
        {
            roughXZ + new GridPosition(-1 , 0),
            roughXZ + new GridPosition(1 , 0),

            roughXZ + new GridPosition(0, 1),
            roughXZ + new GridPosition(0, -1),

            roughXZ + new GridPosition(oddRow ? 1 : -1, 1),
            roughXZ + new GridPosition(oddRow ? 1 : -1, -1),
        };
        GridPosition closetGridPos = roughXZ;
        foreach(GridPosition pos in neighbourGridPositions)
        {
            if(Vector3.Distance(worldP0sition, GetWorldPosition(pos)) < Vector3.Distance(worldP0sition, GetWorldPosition(closetGridPos)))
            {
                closetGridPos = pos;
            }
        }
        return closetGridPos; 
    }
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z ++)
            {
                GridPosition gridPosition = new GridPosition(x, z); 
                Transform debugTrans = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity, LevelGrid.instance.transform);
                GridDebugObject gridDebugObject = debugTrans.GetComponent<GridDebugObject>();
                TGridObject gridObject = GetGridObject(gridPosition);
                gridDebugObject.SetGridObject(gridObject);
            }
        }
    }
    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectsArr[gridPosition.x, gridPosition.z];
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && 
               gridPosition.z >= 0 && 
               gridPosition.x < width && 
               gridPosition.z < height;
    }
}
