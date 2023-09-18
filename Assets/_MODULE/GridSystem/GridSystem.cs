using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystem<TGridObject> 
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
    public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
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
        return new Vector3(gridPosition.x, 0 ,gridPosition.z) * cellSize;
    }
    public GridPosition GetGridPosition(Vector3 gridPosition)
    {
        return new GridPosition(Mathf.RoundToInt(gridPosition.x /cellSize), Mathf.RoundToInt(gridPosition.z/cellSize));
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
