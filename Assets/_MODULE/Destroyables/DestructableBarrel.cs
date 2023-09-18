using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestructableBarrel : Target,  IDestroyable
{
    public static event EventHandler onAnyBarrelDestroy;
    [SerializeField] private ParticleSystem barrelExplodeVFXPrefab;
    [SerializeField] private float explosionRange = 10f;
    [SerializeField] private float explostionForce = 150f;
     [SerializeField] private float damageRadius = 5;
    [SerializeField] private int damageAmount = 90;
    public Transform destroyedBarrelPrefab;
    public override void Start() 
    {
        base.Start();
        LevelGrid.instance.AddDestroyableAtGridPosition(gridPosition, this);
        PathFinding.instance.SetGridPositionWalkable(gridPosition, false);
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public override void Damage(int amount = 0)
    {
        Destroy();
    }
    public void Destroy()
    {
        Debug.Log("DESTRUCTABLE BARREL: start to destroy");
        Transform trans = Instantiate(destroyedBarrelPrefab, transform.position, transform.rotation);
        ApplyExplosionObjectParts(trans, explostionForce, transform.position, explosionRange);
        ApplyExplosionEffects();
        LevelGrid.instance.RemoveTargetAtGridPosition(gridPosition, this);
        LevelGrid.instance.RemoveDestroyableAtGridPosition(gridPosition, this);
        PathFinding.instance.SetGridPositionWalkable(gridPosition, true);
        onAnyBarrelDestroy?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    private void ApplyExplosionEffects()
    {
        Instantiate(barrelExplodeVFXPrefab, transform.position + Vector3.up , Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var c in colliders)
        {
            if (c.TryGetComponent<UnitControl>(out UnitControl unit))
            {
                unit.Damage(damageAmount);
            }
            if (c.gameObject != this.gameObject)
            {
                if (c.TryGetComponent<IDestroyable>(out IDestroyable destroyable))
                {
                    destroyable.Destroy();
                }
            }
        }
    }

    public void ApplyExplosionObjectParts(Transform root, float explostionForce, Vector3 explosionPos, float explosionRange)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explostionForce, explosionPos, explosionRange);
            }
            // ApplyExplosionObjectParts(child, explostionForce, explosionPos, explosionRange);
        }
    }
    
}
