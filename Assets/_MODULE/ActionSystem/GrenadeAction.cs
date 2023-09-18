using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    public enum GrenadeState
    {
        GetGrenade,
        Throw,
        CoolOff
    }
    [SerializeField] private int maxThrowDistance;
    [SerializeField] private Transform throwPosition;
    [SerializeField] private GrenadeProjectile projectile_grenadePrefab;
    [SerializeField] private float rotateSpeed = 40;

    public event EventHandler onThrowGrenade;
    private bool canSpawnGrenade = false;
    private bool canThrow = false;
    private Vector3 targetPosition;
    private GrenadeState state;

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }
   
    private void Update()
    {
        if(isActive == false) return;
        var aimDir = (targetPosition - unit.GetWorldPosition()).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDir, rotateSpeed* Time.deltaTime);
    }
    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> listPos = new List<GridPosition>();
        GridPosition unitGridpos = unit.GetGridPosition();
        for(int x = -maxThrowDistance; x <= maxThrowDistance; x ++)
        {
            for(int z = -maxThrowDistance; z <= maxThrowDistance; z ++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridpos = unitGridpos + offsetGridPos;
                if(!LevelGrid.instance.IsValidGridPosition(testGridpos))
                    continue;
                int tmpDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(tmpDistance > maxThrowDistance)
                    continue;
                
                listPos.Add(testGridpos);
            }
        }
        return listPos;
    }
    public void ThrowGrenade()
    {
        canThrow = true;
    }
    public void SpawnGrenade()
    {
        canSpawnGrenade = true;
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        onThrowGrenade?.Invoke(this, EventArgs.Empty);
        targetPosition = LevelGrid.instance.GetWorldPosition(gridPosition);
        StartCoroutine(CoroutineThrowGrenade(gridPosition));
        ActionStart(onActionComplete);
    }
   
    private IEnumerator CoroutineThrowGrenade(GridPosition gridPosition)
    {
        while(canSpawnGrenade == false)
            yield return new WaitForEndOfFrame();
        GrenadeProjectile grenadeProjectile = Instantiate(projectile_grenadePrefab, throwPosition.position, Quaternion.identity);
        while(canThrow == false)
            yield return new WaitForEndOfFrame();

        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);
        grenadeProjectile.Throw();
        canSpawnGrenade = false;
        canThrow = false;
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
    
}
