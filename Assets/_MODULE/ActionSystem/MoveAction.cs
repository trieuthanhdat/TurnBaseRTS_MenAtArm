using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler onStartMoveAction;
    public event EventHandler onStopMoveAction;
    [SerializeField] float rotateSpeed = 4f;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float stoppingDistance = .1f;
    [SerializeField] int maxMoveDistance = 5;

    private List<Vector3> listPosition;
    private int currentPositionIndex;
    
    private void Update()
    {
        MoveUnit();
    }

    private void MoveUnit()
    {
        if(!isActive) return;

        Vector3 targetPosition = listPosition[currentPositionIndex];
        bool canMove = Vector3.Distance(transform.position, targetPosition) > stoppingDistance;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed* Time.deltaTime);

        if(canMove)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }else
        {
            currentPositionIndex ++ ;
            if(currentPositionIndex >= listPosition.Count)
            {
                onStopMoveAction?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        List<GridPosition> positions = PathFinding.instance.FindPaths(unit.GetGridPosition(), targetPosition, out int pathLength);
        listPosition = new List<Vector3> ();

        foreach(var pos in positions)
        {
            listPosition.Add(LevelGrid.instance.GetWorldPosition(pos));
        }
        currentPositionIndex = 0;
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
                if(LevelGrid.instance.HasAnyTargetOnGridPosition(testGridpos))
                    continue;
                if(LevelGrid.instance.HasAnyDestroyableOnGridPosition(testGridpos))
                    continue;
                if(!PathFinding.instance.IsGridPositionWalkable(testGridpos))
                    continue;
                if(!PathFinding.instance.ValidatePathsAndLength(unitGridpos, testGridpos, maxMoveDistance * PathFinding.instance.GetMoveStaightCost()))
                {
                    continue;
                }
                listPos.Add(testGridpos);
            }
        }
        return listPos;
    }
    public override string GetActionName()
    {
        return "Move";
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCount = unit.GetAction<ShootAction>().GetTargetCountAtPoisition(gridPosition);
        Debug.Log("MOVE ACTION: target count " + targetCount + " at "+ gridPosition.ToString());
        return new EnemyAIAction 
        {
            gridPosition = gridPosition,
            actionValue = targetCount * 10,
        };
    }
    
}
