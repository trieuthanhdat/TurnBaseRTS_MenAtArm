using System;
using System.Collections;
using UnityEngine;

public class UnitControl : Target
{
    [SerializeField] private const int MAX_ACTION_POINTS = 2;
    [SerializeField] private bool isEnemy = false;
    public static event EventHandler OnAnyActionChange;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    private BaseAction[] baseActions;
    private HealthSystem healthSystem;
    private int actionPoint = MAX_ACTION_POINTS;
    
    private void Awake() 
    {
        baseActions =  GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }
    public override void Start() 
    {
        base.Start();
        TurnSystem.instance.OnTurnChange += TurnSystem_OnTurnChange;
        healthSystem.onDead += HealthSystem_OnDead;
        StartCoroutine(CoroutineInvokeEvents());
    }
    IEnumerator CoroutineInvokeEvents()
    {
        while(UnitManager.instance.HasRegisterEvents == false)
            yield return new WaitForEndOfFrame();
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if(IsEnemy() && !TurnSystem.instance.IsPlayerTurn() ||
           (!IsEnemy() && TurnSystem.instance.IsPlayerTurn()))
        {
            actionPoint = MAX_ACTION_POINTS;
            OnAnyActionChange?.Invoke(this, EventArgs.Empty);
        }
        
    }

    private void Update()
    {
        UpdateGridPosition();
    }

    private void UpdateGridPosition()
    {
       GridPosition newGridpos = LevelGrid.instance.GetGridPosition(transform.position);
       if(newGridpos != gridPosition)
       {
            GridPosition oldgridPos = gridPosition;
            gridPosition = newGridpos;

            LevelGrid.instance.UnitMoveToGridPosition(this, oldgridPos, newGridpos);
       }
        
    }
    public T GetAction<T>() where T: BaseAction
    {
        foreach(BaseAction baseAction in baseActions)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }
    
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public BaseAction[] GetBaseActions()
    {
        return baseActions;
    }
    public int GetActionPoint()
    {
        return actionPoint;
    }
    public bool IsEnemy()
    {
        return isEnemy;
    }
    public override Vector3 GetWorldPosition()
    {
        return gameObject.transform.position;
    }
    public override void Damage(int amount = 0)
    {
        healthSystem.Damage(amount);
    }
    public bool TrySpendActionPoint(BaseAction baseAction)
    {
        if(CanSpendActionPoint(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            return true;
        }else
        {
            return false;
        }
    }
    public bool CanSpendActionPoint(BaseAction baseAction)
    {
        return actionPoint >= baseAction.GetActionPointCost();
    }
    private void SpendActionPoints(int amount)
    {
        actionPoint -= amount;
        OnAnyActionChange?.Invoke(this, EventArgs.Empty);
    }
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
        LevelGrid.instance.RemoveTargetAtGridPosition(gridPosition, this);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }
    public float GetHealthProportion()
    {
        return healthSystem.GetHealthProportion();
    }

}
