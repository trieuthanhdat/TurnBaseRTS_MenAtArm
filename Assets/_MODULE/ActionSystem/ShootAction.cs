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
    [SerializeField] private int damageAmount  = 40;
    
    [SerializeField] private int maxShootDistance = 4;
    [SerializeField] private float rotateSpeed = 10f;
    private ShootingState state;
    private float stateTimer = 0;
    private UnitControl targetUnit;
    private bool canShoot = false;
    public event EventHandler<OnShootEventArgs> onShoot;
    public class OnShootEventArgs : EventArgs
    {
        public UnitControl targetUnit;
        public UnitControl shootingUnit;
    }

    public UnitControl GetTargetUnit()
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
                var aimDir = targetUnit.GetWorldPosition();
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

    private void Shoot()
    {
        onShoot?.Invoke(this, new OnShootEventArgs{
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        targetUnit.Damage(damageAmount);
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

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> listPos = new List<GridPosition>();
        GridPosition unitGridpos = unit.GetGridPosition();
        for(int x = -maxShootDistance; x <= maxShootDistance; x ++)
        {
            for(int z = -maxShootDistance; z <= maxShootDistance; z ++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridpos = unitGridpos + offsetGridPos;
                int testDist = Mathf.Abs(x) + Mathf.Abs(z);
                if(!LevelGrid.instance.IsValidGridPosition(testGridpos))
                    continue;
                if(testDist > maxShootDistance)
                    continue;
                if(!LevelGrid.instance.HasAnyUnitOnGridPosition(testGridpos))
                    continue;
                UnitControl targetUnit = LevelGrid.instance.GetUnitOnGridPosition(testGridpos);
                if(targetUnit.IsEnemy() == unit.IsEnemy()) //Both unit on the same team
                    continue;

                listPos.Add(testGridpos);
            }
        }
        return listPos;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.instance.GetUnitOnGridPosition(gridPosition);

        state = ShootingState.Aim;
        canShoot = true;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        ActionStart(onActionComplete);
    }

}
