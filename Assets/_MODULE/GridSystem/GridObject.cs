using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<UnitControl> unitList = new List<UnitControl>();
    public List<UnitControl> UnitList () {return unitList;}
    public void AddUnit(UnitControl u)
    {
        unitList.Add(u);
    }
    public void RemoveUnit(UnitControl u)
    {
        unitList.Remove(u);
    }
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridpos)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridpos;
        unitList = new List<UnitControl>();
    }
    public override string ToString()
    {
        string unitStr = "" ;
        foreach(UnitControl u in unitList)
        {
            unitStr += u + "\n";
        }
        return gridPosition.ToString() + unitStr;
    }
    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }
    public UnitControl GetUnit()
    {
        if(HasAnyUnit())
        {
            return unitList[0];
        }
        return null;
    }
}
