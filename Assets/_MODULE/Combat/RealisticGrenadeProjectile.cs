using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealisticGrenadeProjectile : MonoBehaviour
{
   public static event EventHandler onAnyGrenadeExplode;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem grenadeExplodeVFXPrefab;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    [SerializeField] private float throwForce = 30f;
    [SerializeField] private float throwUpwardForce = 12f;
    [SerializeField] private float damageRadius = 4f;
    [SerializeField] private int damageAmount = 40;
    public event Action onGrenadeBehaviourCompleted;

    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private bool isFlying = false;
    private float startTime;
    private float journeyLength;
    private UnitControl thrower;
    private Rigidbody rigidbody;

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBoom, UnitControl thrower)
    {
        // Initialize the grenade as non-flying
        isFlying = false;
        this.thrower = thrower;
        targetPosition = LevelGrid.instance.GetWorldPosition(targetGridPosition);
        onGrenadeBehaviourCompleted = onGrenadeBoom;
        rigidbody = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    public void LaunchGrenade()
    {
        // Calculate direction to the target
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // Calculate horizontal distance
        float horizontalDistance = Vector3.Distance(targetPosition, transform.position);

        // Calculate vertical distance
        float verticalDistance = targetPosition.y - transform.position.y;

        // Calculate required launch angle (in radians)
        float launchAngle = Mathf.Atan((horizontalDistance * throwUpwardForce) / (horizontalDistance + 0.5f * Physics.gravity.magnitude * throwForce * throwForce));

        // Adjust throwUpwardForce based on vertical distance
        float adjustedUpwardForce = throwUpwardForce * Mathf.Clamp01(verticalDistance / horizontalDistance);

        // Calculate initial velocity
        float initialVelocity = horizontalDistance / (Mathf.Cos(launchAngle) * Mathf.Sqrt((2 * horizontalDistance * Mathf.Tan(launchAngle)) / Physics.gravity.magnitude));

        // Calculate the initial velocity vector
        Vector3 initialVelocityVector = directionToTarget * initialVelocity + Vector3.up * adjustedUpwardForce;

        // Apply the force as an impulse
        rigidbody.AddForce(initialVelocityVector, ForceMode.Impulse);
    }



    private void UpdateTrailRenderer()
    {
        if (trailRenderer != null)
        {
            trailRenderer.AddPosition(transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(3f);
        isFlying = false;

        if (trailRenderer != null)
        {
            trailRenderer.emitting = false; // Stop emitting the trail
        }

        Collider[] colliders = Physics.OverlapSphere(targetPosition, damageRadius);

        foreach (var c in colliders)
        {
            if (c.TryGetComponent<UnitControl>(out UnitControl unit))
            {
                unit.Damage(damageAmount);
            }
        }

        onAnyGrenadeExplode?.Invoke(this, EventArgs.Empty);
        Instantiate(grenadeExplodeVFXPrefab, targetPosition + Vector3.up, Quaternion.identity);
        onGrenadeBehaviourCompleted?.Invoke();

        // Destroy the grenade
        Destroy(gameObject);
    }

    private void Update()
    {
        if(isFlying)
            UpdateTrailRenderer();
    }
}
