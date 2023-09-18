using System.Collections.Generic;
using Unity.VisualScripting;

public class GridObject
{
    private GridSystemHex<GridObject> GridSystemHex;
    private GridPosition gridPosition;
    private List<Target> targetList;
    private List<IDestroyable> destroyableList;
    public List<Target> TargetList () {return targetList;}
    public List<IDestroyable> DestroyableList () {return destroyableList;}

    private IInteractable interactable;
    public IInteractable GetInteractable()
    {
        return interactable;
    }
    public void SetInteractable(IInteractable interactable)
    {
        this.interactable = interactable;
    }
    public void ClearInteractable()
    {
        this.interactable = null;
    }

    public void AddTarget(Target u)
    {
        targetList.Add(u);
    }
    public void AddDestroyable(IDestroyable u)
    {
        destroyableList.Add(u);
    }
    public void RemoveDestroyable(IDestroyable u)
    {
        destroyableList.Remove(u);
    }
    public void RemoveTarget(Target u)
    {
        targetList.Remove(u);
    }
    public GridObject(GridSystemHex<GridObject> GridSystemHex, GridPosition gridpos)
    {
        this.GridSystemHex = GridSystemHex;
        this.gridPosition = gridpos;
        targetList = new List<Target>();
        destroyableList = new List<IDestroyable>();
    }
    public override string ToString()
    {
        string targetStr = "" ;
        foreach(Target u in targetList)
        {
            targetStr += u + "\n";
        }
        return gridPosition.ToString() + targetStr;
    }
    public bool HasAnyTarget()
    {
        return targetList.Count > 0;
    }
    public bool HasAnyUnit()
    {
        foreach(var unit in targetList)
        {
            if(unit.GetComponent<UnitControl>()!= null)
                return true;
        }
        return false;
    }
    public bool HasAnyDestroyables()
    {
        return destroyableList.Count > 0;
    }

    public Target GetTarget()
    {
        if(HasAnyTarget())
        {
            return targetList[0];
        }
        return null;
    }
    public IDestroyable GetDestroyable()
    {
        if(HasAnyDestroyables())
        {
            return destroyableList[0];
        }
        return null;
    }
}
