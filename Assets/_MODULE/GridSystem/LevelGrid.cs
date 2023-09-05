using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelGrid : MonoSingleton<LevelGrid>
{
    [SerializeField] Transform gridDebugPrefab;

    private GridSystem gridSystem;
    private void Awake() 
    {
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugPrefab);
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
