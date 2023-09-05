using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoSingleton<GridSystemVisual>
{

    public enum GridVisualStyle
    {
        White,
        Red,
        Blue,
        Green,
        Yellow,
        RedSoft
    }
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualStyle gridVisualStyle;
        public Material material;
    }
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;
    [SerializeField] private Transform gridSystemVisualPrefab;
    private GridSystemVisualSingle[,] gridSystemVisualSingleArr;
    public GridSystemVisualSingle[,] GridSystemVisualSingleArr { get => gridSystemVisualSingleArr; set => gridSystemVisualSingleArr = value; }

    private void Start() 
    {
        gridSystemVisualSingleArr = new GridSystemVisualSingle[LevelGrid.instance.GetWidth(), LevelGrid.instance.GetHeight()];
        for(int x  = 0; x < LevelGrid.instance.GetWidth(); x ++)
        {
            for(int z = 0; z < LevelGrid.instance.GetHeight(); z ++)
            {
                GridPosition gridpos = new GridPosition(x, z);
                Transform gridVisual = GameObject.Instantiate(gridSystemVisualPrefab, LevelGrid.instance.GetWorldPosition(gridpos), Quaternion.identity);
                gridSystemVisualSingleArr[x, z] = gridVisual.GetComponent<GridSystemVisualSingle>();
            }
        }
        UnitAction.instance.OnSelectedAction += UnitAction_OnSelectedAction;
        LevelGrid.instance.onAnyUnitMoveGridPosition += LevelGrid_OnAnyUnitMoveGridPosition;
    }

    private void LevelGrid_OnAnyUnitMoveGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitAction_OnSelectedAction(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    
    public void UpdateGridVisual()
    {
        HideAllGridPosition();
        BaseAction baseAction = UnitAction.instance.GetSelectedAction();
        if(baseAction == null) return;
        UnitControl selectedUnit = UnitAction.instance.GetSelectedUnit();
        GridVisualStyle style ;
        switch(baseAction)
        {
            default:
            case MoveAction moveAction:
                style = GridVisualStyle.White;
                break;
            case ShootAction shootAction:
                style = GridVisualStyle.RedSoft;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualStyle.Red);
                break;
            case SpinAction spinAction:
                style = GridVisualStyle.Blue;
                break;
        }
        ShowGridPositionList(baseAction.GetValidActionGridPosition(), style);
    }
    public void HideAllGridPosition()
    {
        foreach(var item in gridSystemVisualSingleArr)
        {
            item.Hide();
        }
    }
    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualStyle gridVisualStyle)
    {
        List<GridPosition> listGridPosition = new List<GridPosition>();
        for(int x = -range; x <= range; x ++)
        {
            for(int z = -range; z <= range; z++)
            {
                GridPosition tmpGridPos = gridPosition + new GridPosition(x, z);
                int checkDistance = Mathf.Abs(x)+ Mathf.Abs(z);

                if(!LevelGrid.instance.IsValidGridPosition(tmpGridPos))
                    continue;
                if(checkDistance > range)
                    continue;

                listGridPosition.Add(tmpGridPos);
            }
        }
        ShowGridPositionList(listGridPosition, gridVisualStyle);
    }
    public void ShowGridPositionList(List<GridPosition> listGridpos, GridVisualStyle gridVisualStyle)
    {
        foreach(var item in listGridpos)
        {
            gridSystemVisualSingleArr[item.x, item.z].Show(GetGridVisualMaterial(gridVisualStyle));
        }
    }
    public Material GetGridVisualMaterial(GridVisualStyle gridVisualStyle)
    {
        foreach(var item in gridVisualTypeMaterials)
        {
            if(item.gridVisualStyle == gridVisualStyle)
            {
                return item.material;
            }
        }
        Debug.Log("GRID VISUAL SYSTEM: could not find material with"+ gridVisualStyle);
        return null;
    }
}
