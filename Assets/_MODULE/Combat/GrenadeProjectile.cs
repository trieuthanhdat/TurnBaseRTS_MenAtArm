using System;
using System.Collections;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
     public static event EventHandler onAnyGrenadeExplode;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem grenadeExplodeVFXPrefab;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    [SerializeField] private float moveSpeed = 200f;
    [SerializeField] private float damageRadius = 4f;
    [SerializeField] private int damageAmount = 40;
    public event Action onGrenadeBehaviourCompleted;

    private Vector3 targetPosition;
    private float totalDistance = 0;
    private Vector3 positionXZ = Vector3.one;
    private bool canThrow = false;
    public void Throw()
    {
        canThrow = true;
    }
    private void Update() 
    {
        if(!canThrow) return;
        
        Vector3 moveDir = (targetPosition - positionXZ).normalized;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;
        
        float animDist = Vector3.Distance(positionXZ, targetPosition);
        float ditanceNormalized = 1 - animDist / totalDistance;

        float positionY = arcYAnimationCurve.Evaluate(ditanceNormalized);
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float distanceReachTarget  = 0.2f;

        if(Vector3.Distance(positionXZ, targetPosition) < distanceReachTarget)
        {
            Collider[] colliders = Physics.OverlapSphere(targetPosition, damageRadius);
            
            foreach(var c in colliders)
            {
                if(c.TryGetComponent<UnitControl>(out UnitControl unit))
                {
                    unit.Damage(damageAmount);
                }
                if(c.TryGetComponent<IDestroyable>(out IDestroyable destroyable))
                {
                    destroyable.Destroy();
                }
            }
            onAnyGrenadeExplode?.Invoke(this, EventArgs.Empty);
            Instantiate(grenadeExplodeVFXPrefab, targetPosition + Vector3.up , Quaternion.identity);
            Destroy(gameObject);
            onGrenadeBehaviourCompleted();
        }
    }
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBoom, UnitControl unit = null)
    {
        onGrenadeBehaviourCompleted = onGrenadeBoom;
        targetPosition = LevelGrid.instance.GetWorldPosition(targetGridPosition);
        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
