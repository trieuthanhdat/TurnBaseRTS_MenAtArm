using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int height;
    private int width;
    private float cellSize;

    private GridObject[,] gridObjectsArr;
    public int Width 
    {
        get{return width;}
    }
    public int Height 
    {
        get{return height;}
    }
    public GridSystem(int w, int h, float cellSize)
    {
        this.width = w;
        this.height = h;
        this.cellSize = cellSize;
        gridObjectsArr = new GridObject[w, h];
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z ++)
            {
                GridPosition gridpos = new GridPosition(x, z); 
                gridObjectsArr[x, z] = new GridObject(this, gridpos);
                Debug.DrawLine(GetWorldPosition(gridpos), GetWorldPosition(gridpos) + Vector3.right * 0.2f, Color.white, 1000);
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
               if(gridDebugObject) gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }
    public GridObject GetGridObject(GridPosition gridPosition)
    {
        if(IsValidGridPosition(gridPosition) == false) return null;
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
