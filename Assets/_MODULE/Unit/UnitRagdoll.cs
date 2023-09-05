using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private int exploseForce  = 300;
    [SerializeField] private int exploseRange  = 10;
    [SerializeField] private Transform ragdollRootBone;
    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransform(originalRootBone, ragdollRootBone);
        ApplyExplosion(originalRootBone, exploseForce, transform.position, exploseRange);
    }
    private void MatchAllChildTransform(Transform root, Transform clone)
    {
        foreach(Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if(cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllChildTransform(child, cloneChild);
            }
        }
    }
    private void ApplyExplosion(Transform root, float explostionForce, Vector3 explosionPos, float explosionRange)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explostionForce, explosionPos, explosionRange);
            }
            ApplyExplosion(child, explostionForce, explosionPos, explosionRange);
        }
    }
}
