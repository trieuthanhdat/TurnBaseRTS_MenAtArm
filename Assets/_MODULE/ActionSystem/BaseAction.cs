using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnActionCompleted;
    public Sprite actionSprite;
    protected UnitControl unit;
    protected bool isActive = false;
    protected Action onActionComplete;
    protected virtual void Awake()
    {
        unit = GetComponent<UnitControl>();
        isActive= false;
    }
    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    public virtual bool IsValidGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridpos = GetValidActionGridPosition();
        return validGridpos.Contains(gridPosition);
    }
    public virtual Sprite GetActionSprite()
    {
        return actionSprite;
    }
    public abstract List<GridPosition> GetValidActionGridPosition();
    public virtual int GetActionPointCost()
    {
        return 1;
    }
    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }
    protected void ActionComplete()
    {
        isActive= false;
        onActionComplete();
        OnActionCompleted?.Invoke(this, EventArgs.Empty);
    }
    public UnitControl GetUnit()
    {
        return unit;
    }
}
