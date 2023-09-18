using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoSingleton<LevelGrid>
{
    [SerializeField] Transform gridDebugPrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2;
    public event EventHandler<OnAnyUnitMovedGridPositionEventArgs> OnAnyUnitMovedGridPosition;
    public class OnAnyUnitMovedGridPositionEventArgs : EventArgs
    {
        public UnitControl unit;
        public GridPosition fromGridPosition;
        public GridPosition toGridPosition;
    }

    private GridSystemHex<GridObject> GridSystemHex;
    private void Awake() 
    {
        GridSystemHex = new GridSystemHex<GridObject>(width, height, cellSize, (GridSystemHex<GridObject> g, GridPosition gridpos) => new GridObject(g, gridpos));
        // GridSystemHex.CreateDebugObjects(gridDebugPrefab);
    }
    private void Start()
    {
        PathFinding.instance.Setup(width, height, cellSize);
    }
    public void ClearInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GridSystemHex.GetGridObject(gridPosition);
        gridObject.ClearInteractable();
    }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridpos)
    {
        GridObject gridObject = GridSystemHex.GetGridObject(gridpos); 
        return gridObject.GetInteractable();
    }
    public void SetInteractableAtGridPosition(GridPosition gridpos, IInteractable interactable)
    {
        GridObject gridObject = GridSystemHex.GetGridObject(gridpos); 
        gridObject.SetInteractable(interactable);
    }
    public void AddTargetAtGridPosition(GridPosition gridpos, Target unit)
    {
        GridObject gridOBj = GridSystemHex.GetGridObject(gridpos);
        if(gridOBj != null) gridOBj.AddTarget(unit);
    }
    public void AddDestroyableAtGridPosition(GridPosition gridpos, IDestroyable destroyable)
    {
        GridObject gridOBj = GridSystemHex.GetGridObject(gridpos);
        if(gridOBj != null) gridOBj.AddDestroyable(destroyable);
    }

    public List<Target> GetUnitListAtGridPosition(GridPosition gridpos)
    {
        GridObject gridobj = GridSystemHex.GetGridObject(gridpos);
        return gridobj.TargetList();
    }
    public List<IDestroyable> GetDestroyableListAtGridPosition(GridPosition gridpos)
    {
        GridObject gridobj = GridSystemHex.GetGridObject(gridpos);
        return gridobj.DestroyableList();
    }
    public void RemoveTargetAtGridPosition(GridPosition gridpos, Target unit)
    {
        GridObject gridobj = GridSystemHex.GetGridObject(gridpos);
        if(gridobj != null) gridobj.RemoveTarget(unit);
    }
    public void RemoveDestroyableAtGridPosition(GridPosition gridpos, IDestroyable destroyable)
    {
        GridObject gridobj = GridSystemHex.GetGridObject(gridpos);
        if(gridobj != null) gridobj.RemoveDestroyable(destroyable);
    }
    public void UnitMoveToGridPosition(UnitControl unit, GridPosition fromGrid, GridPosition toGrid)
    {
        RemoveTargetAtGridPosition(fromGrid, unit);
        AddTargetAtGridPosition(toGrid, unit);
         OnAnyUnitMovedGridPosition?.Invoke(this, new OnAnyUnitMovedGridPositionEventArgs {
            unit = unit,
            fromGridPosition = fromGrid,
            toGridPosition = toGrid,
        });

    }
    public GridPosition GetGridPosition(Vector3 worldPosition) => GridSystemHex.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => GridSystemHex.GetWorldPosition(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => GridSystemHex.IsValidGridPosition(gridPosition);
    public bool HasAnyTargetOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GridSystemHex.GetGridObject(gridPosition);
        if(gridObject == null) return false;
        return gridObject.HasAnyTarget();
    }
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GridSystemHex.GetGridObject(gridPosition);
        if(gridObject == null) return false;
        return gridObject.HasAnyUnit();
    }
    public bool HasAnyDestroyableOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GridSystemHex.GetGridObject(gridPosition);
        if(gridObject == null) return false;
        return gridObject.HasAnyDestroyables();
    }
    public Target GetTargetOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GridSystemHex.GetGridObject(gridPosition);
        return gridObject.GetTarget();
    }
    
    public UnitControl GetUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GridSystemHex.GetGridObject(gridPosition);
        return gridObject.GetTarget().GetComponent<UnitControl>();
    }
    public IDestroyable GetDestroyableOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GridSystemHex.GetGridObject(gridPosition);
        return gridObject.GetDestroyable();
    }
    public int GetWidth()
    {
        return GridSystemHex.Width;
    }
    public int GetHeight()
    {
        return GridSystemHex.Height;
    }
}
