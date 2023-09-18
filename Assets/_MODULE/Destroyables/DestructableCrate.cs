using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestructableCrate : MonoBehaviour, IDestroyable
{
    [SerializeField] private Transform destroyedCratedPrefab; 
    [SerializeField] private float explosionRange = 10f;
    [SerializeField] private float explostionForce = 150f;
   

    public static event EventHandler onAnyCrateDestroy;
    private GridPosition gridPosition;

    public void Start() 
    {
        gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
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

    public void Destroy()
    {
        Transform trans = Instantiate(destroyedCratedPrefab, transform.position, transform.rotation);
        ApplyExplosionObjectParts(trans, explostionForce, transform.position, explosionRange);
        Destroy(gameObject);
        onAnyCrateDestroy?.Invoke(this, EventArgs.Empty);
    }
}
