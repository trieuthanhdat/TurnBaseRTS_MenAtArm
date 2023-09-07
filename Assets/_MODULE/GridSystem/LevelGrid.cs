using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelGrid : MonoSingleton<LevelGrid>
{
    [SerializeField] Transform gridDebugPrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2;
    public event EventHandler onAnyUnitMoveGridPosition;
    private GridSystem<GridObject> gridSystem;
    private void Awake() 
    {
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridpos) => new GridObject(g, gridpos));
        // gridSystem.CreateDebugObjects(gridDebugPrefab);
    }
    public void AddUnitAtGridPosition(GridPosition gridpos, UnitControl unit)
    {
        GridObject gridOBj = gridSystem.GetGridObject(gridpos);
        if(gridOBj != null) gridOBj.AddUnit(unit);
    }

    public List<UnitControl> GetUnitListAtGridPosition(GridPosition gridpos)
    {
        GridObject gridobj = gridSystem.GetGridObject(gridpos);
        return gridobj.UnitList();
    }
    public void RemoveUnitAtGridPosition(GridPosition gridpos, UnitControl unit)
    {
        GridObject gridobj = gridSystem.GetGridObject(gridpos);
        if(gridobj != null) gridobj.RemoveUnit(unit);
    }
    public void UnitMoveToGridPosition(UnitControl unit, GridPosition fromGrid, GridPosition toGrid)
    {
        RemoveUnitAtGridPosition(fromGrid, unit);
        AddUnitAtGridPosition(toGrid, unit);
        onAnyUnitMoveGridPosition?.Invoke(this, EventArgs.Empty);
    }
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        if(gridObject == null) return false;
        return gridObject.HasAnyUnit();
    }
    public UnitControl GetUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
    public int GetWidth()
    {
            return gridSystem.Width;
    }
    public int GetHeight()
    {
            return gridSystem.Height;
    }
}
