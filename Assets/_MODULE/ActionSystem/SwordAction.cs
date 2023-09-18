using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwordAction : BaseAction
{
    private enum SwingingSwordState
    {
        SwingBeforeHit,
        SwingAfterHit
    }
    [SerializeField] private int maxSwordDistance = 1;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private int damageAmount  = 100;
    public static event EventHandler onAnySwordHit;
    public event EventHandler onSwordActionStart;
    public event EventHandler onSwordActionEnd;
    private SwingingSwordState state;
    private float stateTimer = 0;
    private Target targetUnit;
    public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
    {
        targetUnit = LevelGrid.instance.GetTargetOnGridPosition(gridPosition);
        state = SwingingSwordState.SwingBeforeHit;
        float SwingStateTime = 0.7f;
        stateTimer = SwingStateTime;
        onSwordActionStart?.Invoke(this, EventArgs.Empty);
        ActionStart(onSpinComplete);
    }

    void Update()
    {
        if(isActive)
        {
            ProcessState();
        }
    }

    private void ProcessState()
    {
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case SwingingSwordState.SwingBeforeHit:
            Debug.Log("TARGET :"+targetUnit.name);
                var aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, rotateSpeed* Time.deltaTime);
                break;
            case SwingingSwordState.SwingAfterHit:
                
                break;
        }
        if(stateTimer <= 0f)
        {
            NextState();
        }
    }
    private void NextState()
    {
        switch (state)
        {
            case SwingingSwordState.SwingBeforeHit:
                state = SwingingSwordState.SwingAfterHit;
                float SwingStateTime = 0.7f;;
                stateTimer = SwingStateTime;
                onAnySwordHit?.Invoke(this, EventArgs.Empty);
                targetUnit.Damage(damageAmount);
                break;
            case SwingingSwordState.SwingAfterHit:
                onSwordActionEnd?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }
    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> listPos = new List<GridPosition>();
        GridPosition unitGridpos = unit.GetGridPosition();
        for(int x = -maxSwordDistance; x <= maxSwordDistance; x ++)
        {
            for(int z = -maxSwordDistance; z <= maxSwordDistance; z ++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridpos = unitGridpos + offsetGridPos;
                if(!LevelGrid.instance.IsValidGridPosition(testGridpos))
                    continue;
                if(!LevelGrid.instance.HasAnyTargetOnGridPosition(testGridpos))
                    continue;
                Target targetUnit = LevelGrid.instance.GetTargetOnGridPosition(testGridpos);
                if(targetUnit.GetComponent<UnitControl>() != null)
                    if(targetUnit.GetComponent<UnitControl>().IsEnemy() == unit.IsEnemy())
                        continue;

                listPos.Add(testGridpos);
            }
        }
        return listPos;
    }
    public override string GetActionName()
    {
        return "Sword";
    }
    public override int GetActionPointCost()
    {
        return 1;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction 
        {
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }
    public int GetMaxShootDistance()
    {
        return maxSwordDistance;
    }
}
