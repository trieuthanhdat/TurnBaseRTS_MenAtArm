using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler onStartMoveAction;
    public event EventHandler onStopMoveAction;
    [SerializeField] float rotateSpeed = 4f;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float stoppingDistance = .1f;
    [SerializeField] int maxMoveDistance = 5;

    private Vector3 targetPosition;
    protected override void Awake() 
    {
        base.Awake();
        targetPosition  = transform.position;
    }
    private void Update()
    {
        MoveUnit();
    }

    private void MoveUnit()
    {
        if(!isActive) return;

        bool canMove = Vector3.Distance(transform.position, targetPosition) > stoppingDistance;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if(canMove)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }else
        {
            onStopMoveAction?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed* Time.deltaTime);
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        this.targetPosition = LevelGrid.instance.GetWorldPosition(targetPosition);
        onStartMoveAction?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }
   
    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> listPos = new List<GridPosition>();
        GridPosition unitGridpos = unit.GetGridPosition();
        for(int x = -maxMoveDistance; x <= maxMoveDistance; x ++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z ++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridpos = unitGridpos + offsetGridPos;
                if(!LevelGrid.instance.IsValidGridPosition(testGridpos))
                    continue;
                if(unitGridpos == testGridpos)
                    continue;                      
                if(LevelGrid.instance.HasAnyUnitOnGridPosition(testGridpos))
                    continue;
                    
                listPos.Add(testGridpos);
            }
        }
        return listPos;
    }
    public override string GetActionName()
    {
        return "Move";
    }

}
