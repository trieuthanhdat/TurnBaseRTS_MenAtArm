using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    [SerializeField] float spinAddAmount = 360;
    private float totalSpinAmount = 0;

    public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
    {
        totalSpinAmount = 0;
        ActionStart(onSpinComplete);
    }
    void Update()
    {
        if(isActive)
        {
            Spin();
        }
    }

    private void Spin()
    {
        transform.eulerAngles += new Vector3(0, spinAddAmount * Time.deltaTime, 0);
        totalSpinAmount += spinAddAmount * Time.deltaTime;
        if(totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }
    }
    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> listPos = new List<GridPosition>();
        GridPosition unitGridpos = unit.GetGridPosition();
        listPos.Add(unitGridpos);
        return listPos;
    }
    public override string GetActionName()
    {
        return "Spin";
    }
    public override int GetActionPointCost()
    {
        return 2;
    }
}
