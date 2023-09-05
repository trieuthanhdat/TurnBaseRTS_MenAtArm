using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoSingleton<GridSystemVisual>
{
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
    }
    private void Update() 
    {
        UpdateGridVisual();
    }
    public void UpdateGridVisual()
    {
        HideAllGridPosition();
        BaseAction baseAction = UnitAction.instance.GetSelectedAction();
        if(baseAction == null) return;
        ShowGridPositionList(baseAction.GetValidActionGridPosition());
    }
    public void HideAllGridPosition()
    {
        foreach(var item in gridSystemVisualSingleArr)
        {
            item.Hide();
        }
    }
    public void ShowGridPositionList(List<GridPosition> listGridpos)
    {
        foreach(var item in listGridpos)
        {
            gridSystemVisualSingleArr[item.x, item.z].Show();
        }
    }
}
