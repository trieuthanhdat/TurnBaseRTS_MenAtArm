using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum ShootingState
    {
        Aim,
        Shoot,
        CoolOff
    }
    [SerializeField] LayerMask obstacleLayerMask;
    [SerializeField] private int damageAmount  = 40;
    
    [SerializeField] private int maxShootDistance = 4;
    [SerializeField] private float rotateSpeed = 10f;
    private ShootingState state;
    private float stateTimer = 0;
    private Target targetUnit;
    private bool canShoot = false;
    public event EventHandler<OnShootEventArgs> onShoot;
    public static event EventHandler onAnyShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Target targetUnit;
        public Target shootingUnit;
    }
    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }
    public Target GetTargetUnit()
    {
        return targetUnit;
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
            case ShootingState.Aim:
                var aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, rotateSpeed* Time.deltaTime);
                break;
            case ShootingState.Shoot:
                if(canShoot)
                {
                    Shoot();
                    canShoot = false;
                }
                break;
            case ShootingState.CoolOff:
                
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
            case ShootingState.Aim:
                state = ShootingState.Shoot;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case ShootingState.Shoot:
                state = ShootingState.CoolOff;
                float coolOffStatetime = 0.5f;
                stateTimer = coolOffStatetime;
                break;
            case ShootingState.CoolOff:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        onShoot?.Invoke(this, new OnShootEventArgs{
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        onAnyShoot?.Invoke(this, EventArgs.Empty);
        targetUnit.Damage(damageAmount);
    }

    

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        GridPosition gridPositionUnit = unit.GetGridPosition();
        return GetValidActionGridPosition(gridPositionUnit);
    }
    public List<GridPosition> GetValidActionGridPosition(GridPosition unitGridpos)
    {
        List<GridPosition> listPos = new List<GridPosition>();
        for(int x = -maxShootDistance; x <= maxShootDistance; x ++)
        {
            for(int z = -maxShootDistance; z <= maxShootDistance; z ++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridpos = unitGridpos + offsetGridPos;
                if(!LevelGrid.instance.IsValidGridPosition(testGridpos))
                    continue;
                int tmpDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(tmpDistance > maxShootDistance)
                    continue;
                if(unit.gameObject.CompareTag("Enemy"))
                {
                    if(LevelGrid.instance.HasAnyDestroyableOnGridPosition(testGridpos))
                        continue;
                }
                if(!LevelGrid.instance.HasAnyTargetOnGridPosition(testGridpos)) 
                    continue;
                Target targetUnit = LevelGrid.instance.GetTargetOnGridPosition(testGridpos);
                if(targetUnit.GetComponent<UnitControl> () != null)
                    if(targetUnit.GetComponent<UnitControl>().IsEnemy() == unit.IsEnemy()) // if Friend
                        continue;
                    
                float unitShoulderHeight = 1.7f;
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                if(Physics.Raycast(unit.GetWorldPosition() + Vector3.up * unitShoulderHeight, shootDir, Vector3.Distance(unit.GetWorldPosition(), targetUnit.GetWorldPosition()), obstacleLayerMask))
                    continue; //block by obstacle

                
                listPos.Add(testGridpos);
            }
        }
        return listPos;
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.instance.GetTargetOnGridPosition(gridPosition);

        state = ShootingState.Aim;
        canShoot = true;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        ActionStart(onActionComplete);
    }
    public int GetTargetCountAtPoisition(GridPosition gridPosition)
    {
        return GetValidActionGridPosition(gridPosition).Count;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        UnitControl targetUnit = LevelGrid.instance.GetUnitOnGridPosition(gridPosition);

        return new EnemyAIAction 
        {
            gridPosition = gridPosition,
            actionValue = 100 + ( targetUnit != null ? Mathf.RoundToInt((1- targetUnit.GetHealthProportion()) * 100f) : 0)
        };
    }
}
