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
    private GridSystemVisualSingle lastSelectedGridSystemVisualSingle;
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
        DestructableBarrel.onAnyBarrelDestroy += DestructableBarrel_OnAnyDestroyablesKill;
        LevelGrid.instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMoveGridPosition;
    }
    private void Update() 
    {
        if(lastSelectedGridSystemVisualSingle != null)
        {
            lastSelectedGridSystemVisualSingle.HideSelected();
        }
        Vector3 mousePos = MouseControl.GetPosition();
        GridPosition gridPosition = LevelGrid.instance.GetGridPosition(mousePos);
        LevelGrid.instance.GetGridPosition(mousePos);
        if(LevelGrid.instance.IsValidGridPosition(gridPosition))
        {
            lastSelectedGridSystemVisualSingle = gridSystemVisualSingleArr[gridPosition.x, gridPosition.z];
        }
        if(lastSelectedGridSystemVisualSingle != null)
        {
            lastSelectedGridSystemVisualSingle.ShowSelected();
        }
    }
    private void DestructableBarrel_OnAnyDestroyablesKill(object sender, EventArgs e)
    {
        UpdateGridVisual();
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
            case GrenadeAction grenadeAction:
                style = GridVisualStyle.Yellow;
                break;
            case SpinAction spinAction:
                style = GridVisualStyle.Blue;
                break;
            case InteractAction interactAction:
                style = GridVisualStyle.Blue;
                break;
            case SwordAction swordAction:
                style = GridVisualStyle.RedSoft;
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxShootDistance(), GridVisualStyle.Red);
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
    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualStyle gridVisualStyle)
    {
        List<GridPosition> listGridPosition = new List<GridPosition>();
        for(int x = -range; x <= range; x ++)
        {
            for(int z = -range; z <= range; z++)
            {
                GridPosition tmpGridPos = gridPosition + new GridPosition(x, z);
                if(!LevelGrid.instance.IsValidGridPosition(tmpGridPos))
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
