using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    protected GridPosition gridPosition;
    public virtual void Start() 
    {
        gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        transform.position = LevelGrid.instance.GetWorldPosition(gridPosition);
        LevelGrid.instance.AddTargetAtGridPosition(gridPosition, this);
    }
    public virtual Vector3 GetWorldPosition()
    {
        return gameObject.transform.position;
    }
    public virtual void Damage(int amount)
    {
        
    }
}
