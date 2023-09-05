using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitAction : MonoSingleton<UnitAction>
{
    [SerializeField] LayerMask unitLayerMask;
    public event EventHandler OnSelectedUnit;
    public event EventHandler OnSelectedAction;
    public event EventHandler<bool> onBusyChange;
    public event EventHandler onActionStarted;
    private UnitControl selectedUnit;
    private BaseAction selectedAction;
    private bool isBusy = false;
    

    private void Start()
    {
        SelectUnit(selectedUnit);
    }
    void Update()
    {
        if(isBusy) return;
        if(!TurnSystem.instance.IsPlayerTurn()) return;
        if(EventSystem.current.IsPointerOverGameObject()) return;
        if( TryHandleUnitSelection()) return;

        HandleSelectedAction();
    }
    private void HandleSelectedAction()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridpos = LevelGrid.instance.GetGridPosition(MouseControl.GetPosition());
            if(selectedAction)  
            {
                if(!selectedAction.IsValidGridPosition(mouseGridpos))
                    return;
                if(!selectedUnit.TrySpendActionPoint(selectedAction))
                    return;
                SetBusy();
                selectedAction.TakeAction(mouseGridpos, ClearBusy);
                onActionStarted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    private bool TryHandleUnitSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, unitLayerMask))
            {
                if(hitInfo.transform.TryGetComponent<UnitControl>(out UnitControl unit))
                {
                    if(unit == selectedUnit)
                        return false;
                    if(unit.IsEnemy())
                        return false;

                    SelectUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SelectUnit(UnitControl unit)
    {
        selectedUnit = unit;
        SelecteAction(unit.GetMoveAction());
        OnSelectedUnit?.Invoke(this, EventArgs.Empty);
    }
    public void SelecteAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedAction?.Invoke(this, EventArgs.Empty);
    }
    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    public Sprite GetActionSprite()
    {
        return selectedAction.GetActionSprite();
    }
    public UnitControl GetSelectedUnit()
    {
        return selectedUnit;
    }

    public void SetBusy()
    {
        isBusy = true;
        onBusyChange?.Invoke(this, isBusy);
    }
    private void ClearBusy()
    {
        isBusy = false;
        onBusyChange?.Invoke(this, isBusy);
    }
}
