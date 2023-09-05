using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitControl : MonoBehaviour
{
    [SerializeField] private const int MAX_ACTION_POINTS = 2;
    [SerializeField] private bool isEnemy = false;
    public static event EventHandler OnAnyActionChange;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private GridPosition gridPosition;
    private BaseAction[] baseActions;
    private HealthSystem healthSystem;
    private int actionPoint = MAX_ACTION_POINTS;
    
    private void Awake() 
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActions =  GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }
    private void Start() 
    {
        gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.instance.OnTurnChange += TurnSystem_OnTurnChange;
        healthSystem.onDead += HealthSystem_OnDead;
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
            LevelGrid.instance.UnitMoveToGridPosition(this, gridPosition, newGridpos);
            gridPosition = newGridpos;
       }
        
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }
    public SpinAction GetSpinAction()
    {
        return spinAction;
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
    public Vector3 GetWorldPosition()
    {
        return gameObject.transform.position;
    }
    public void Damage(int amount)
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
        LevelGrid.instance.RemoveUnitAtGridPosition(gridPosition, this);
    }
}
