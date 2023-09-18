using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    [SerializeField]private int maxInteractDistance = 1;

    private void Update() 
    {
        if(isActive)
        {

        }
    }
    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction 
        {
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> listPos = new List<GridPosition>();
        GridPosition unitGridpos = unit.GetGridPosition();
        for(int x = -maxInteractDistance; x <= maxInteractDistance; x ++)
        {
            for(int z = -maxInteractDistance; z <= maxInteractDistance; z ++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridpos = unitGridpos + offsetGridPos;
                if(!LevelGrid.instance.IsValidGridPosition(testGridpos))
                    continue;
                IInteractable interactable = LevelGrid.instance.GetInteractableAtGridPosition(testGridpos);
                if(interactable == null) // no interactable here
                    continue;
                listPos.Add(testGridpos);
            }
        }
        return listPos;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractable interactable = LevelGrid.instance.GetInteractableAtGridPosition(gridPosition);
        interactable.Interact(OnInteractComplete);
        ActionStart(onActionComplete);
    }
    private void OnInteractComplete()
    {
        ActionComplete();
    }
    
}
