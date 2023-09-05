using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem bulletHitVFXPrefab;
    [SerializeField] private float moveSpeed = 200f;

    private Vector3 targetPosition;

    public void Setup(Vector3 targetPos)
    {
        this.targetPosition = targetPos;
    }
    private void Update() 
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float distanceAfterMoving  = Vector3.Distance(transform.position, targetPosition);
        if(distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;
            trailRenderer.transform.parent = null;
            Instantiate(bulletHitVFXPrefab, targetPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
